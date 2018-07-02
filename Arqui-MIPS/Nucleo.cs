using System;
using System.Collections.Generic;
using System.Threading;

namespace Arqui_MIPS
{
    public class Nucleo
    {
        private const int CICLOS_TRAER_DE_MEMORIA = 40;

        Contexto contextoEnEjecucion;
        BusDatos busDatos;
        BusInstrucciones busInstrucciones;
        CacheDatos cacheDatos;
        CacheInstrucciones cacheInstrucciones;

        // para sincronizacion
        public Barrier Sync;
        public int quantum;
        public int identificador, CicloActual;
        public Queue<Contexto> colaContextos;
        public List<Contexto> contextosTerminados;

        /*
         * Constructor de la clase
         */
        public Nucleo(Barrier barrera, BusDatos bd, BusInstrucciones bi, int id, int quantumInicial, ref Queue<Contexto> colaContextos, ref List<Contexto> contextosTerminados)
        {
            Sync = barrera;
            busDatos = bd;
            busInstrucciones = bi;
            identificador = id;
            quantum = quantumInicial;
            this.colaContextos = colaContextos;
            this.contextosTerminados = contextosTerminados;

            cacheDatos = new CacheDatos();
            cacheInstrucciones = new CacheInstrucciones();
        }

        /*
         * SetContextoEnEjecucion Desencola el contexto a ejecutar y lo almacena en la variable respectiva
         * 
         */
        public void SetContextoEnEjecucion()
        {
            contextoEnEjecucion = colaContextos.Dequeue();
        }

        /*
         * GetCacheInstrucciones Retorna la referencia a la caché de instrucciones
         * 
         * @return CacheInstrucciones
         */
        public CacheInstrucciones GetCacheInstrucciones()
        {
            return cacheInstrucciones;
        }

        /*
         * GetCacheDatos Retorna la referencia a la caché de datos
         * 
         * @return CacheDatos
         */
        public CacheDatos GetCacheDatos()
        {
            return cacheDatos;
        }

        internal void Iniciar()
        {
            // Se inicia el hilo
            // mientras todavía existan contextos en la cola. Debo pensar en el manejo de esta cola          
            while (colaContextos.Count > 0)
            {
                Sync.SignalAndWait();

                contextoEnEjecucion = colaContextos.Dequeue();
                var pc = contextoEnEjecucion.GetPC();
                //var posicion = contextoEnEjecucion.

                Sync.RemoveParticipant();
            }
            //lock (colaContextos)
            // { //Se bloquea para que solo uno de los núcleos a la vez pueda sacar de la cola
            //    if (colaContextos.Count > 0)
            //    {
            //        contextoEnEjecucion = colaContextos.Dequeue();
            //    }
                //Habría que hacer algo cuando ya no hay contextos para correr
            //}

            //while (true)
            //{
              //  Run();
            //}
        }

        /*
         * Correr Lógica de la ejecución de un hilillo en el procesador
         * ((WIP))
        */
        internal void Run()
        {
            /**Fetch de la instrucción**/
            int[] instruccion = new int[4];

            //Obtener direcciones
            int pcContexto = contextoEnEjecucion.GetPC();
            int nBloque = GetNumeroBloque(pcContexto);
            int nPalabra = GetNumeroPalabra(pcContexto,4);
            int nBloqueEnCache = GetPosicionCache(nBloque);

            //Revisar la caché de instrucciones
            //--Creo que no hace falta bloquear la posición de caché ya que el otro núcleo no tiene porque usarla dado que las instrucciones no se modifican
            if (cacheInstrucciones.GetEtiquetaBloque(nBloqueEnCache) != nBloque)
            {
                /**FALLO DE CACHÉ**/
                //Pedir el bus de instrucciones
                if (Monitor.TryEnter(busInstrucciones))
                {
                    try
                    {
                        //Cargar el bloque de memoria a la caché
                        var bloque = busInstrucciones.BloqueDeMem(nBloque);
                        cacheInstrucciones.SetBloque(nBloqueEnCache, bloque);
                        AvanzarReloj(CICLOS_TRAER_DE_MEMORIA);
                    }
                    finally
                    {
                        //Libera el bus de instrucciones
                        Monitor.Exit(busInstrucciones);
                    }
                }
                else
                {
                    //No se pudo obtener el bus
                    AvanzarReloj(1);    // avanzar reloj es del hilo principal.
                }
            }
            instruccion = cacheInstrucciones.GetPalabraBloque(nBloqueEnCache, nPalabra);
            ManejoInstrucciones(instruccion);
        }

        public bool ManejoInstrucciones(int[] instruccion)
        {
            bool res = false;

            int codigoInstruccion = instruccion[0],
                regFuente1 = instruccion[1],
                regFuente2 = instruccion[2],
                regDest = instruccion[3],
                posMem;

            Contexto contPrincipal = colaContextos.Dequeue();
            string output = "";         //debug
            int pc = contPrincipal.GetPC();
            int nBloque = GetNumeroBloque(pc);
            int nPalabra = GetNumeroPalabra(pc, 4);

            switch (codigoInstruccion)
            {
                case 2:
                    /*
                    JR RX: PC=RX
                    CodOp: 2 RF1: X RF2 O RD: 0 RD o IMM:0
                    */
                    contPrincipal.SetPC(regFuente1);
                    break;
                case 3:
                    /*
                    JAL n, R31=PC, PC = PC+n
                    CodOp: 3 RF1: 0 RF2 O RD: 0 RD o IMM:n
                    */
                    contPrincipal.SetRegistro(31, pc);
                    contPrincipal.AumentarPC(regDest);
                    break;
                case 4:
                    /*
                    BEQZ RX, ETIQ : Si RX = 0 salta 
                    CodOp: 4 RF1: Y RF2 O RD: 0 RD o IMM:n
                    */
                    if (contPrincipal.GetRegistro(regFuente1) == 0)
                    {
                        
                        //salta a la etiqueta indicada por regDest
                    }
                    break;
                case 5:
                    /*
                     BEQNZ RX, ETIQ : Si RX != 0 salta 
                     CodOp: 5 RF1: x RF2 O RD: 0 RD o IMM:n
                     */
                    if (contPrincipal.GetRegistro(regFuente1) != 0)
                    {
                        //salta a la etiqueta indicada por regDest
                        
                    }
                    break;
                case 8:
                    /*
                    DADDI RX, RY, #n : Rx <-- (Ry) + n
                    CodOp: 8 RF1: Y RF2 O RD: x RD O IMM:n
                    */
                    contPrincipal.SetRegistro(regFuente2, contPrincipal.GetRegistro(regFuente1) + regDest);
                    break;
                case 12:
                    /*
                    DMUL RX, RY, #n : Rx <-- (Ry) * (Rz)
                    CodOp: 12 RF1: Y RF2 O RD: z RD o IMM:X
                    */
                    contPrincipal.SetRegistro(regDest,contPrincipal.GetRegistro(regFuente1) * contPrincipal.GetRegistro(regFuente2));

                    break;
                case 14:
                    /*
                    DDIV RX, RY, #n : Rx <-- (Ry) / (Rz)
                    CodOp: 14 RF1: Y RF2 O RD: z RD o IMM:X
                    */
                    contPrincipal.SetRegistro(regDest,contPrincipal.GetRegistro(regFuente1) / contPrincipal.GetRegistro(regFuente2));

                    break;
                case 32:
                    /*
                    DADD RX, RY, #n : Rx <-- (Ry) + (Rz)
                    CodOp: 32 RF1: Y RF2 O RD: x RD o IMM:Rz
                    */
                    contPrincipal.SetRegistro(regDest, contPrincipal.GetRegistro(regFuente1) + contPrincipal.GetRegistro(regFuente2));

                    break;
                case 34:
                    /*
                    DSUB RX, RY, #n : Rx <-- (Ry) - (Rz)
                    CodOp: 34 RF1: Y RF2 O RD: z RD o IMM:X
                    */
                    contPrincipal.SetRegistro(regDest, contPrincipal.GetRegistro(regFuente1) - contPrincipal.GetRegistro(regFuente2));

                    break;
                case 35:
                    /* *
                    * LW Rx, n(Ry)
                    * Rx <- M(n + (Ry))
                    * 
                    * codOp: 35 RF1: Y RF2 O RD: X RD O IMM: n
                    * */
                    posMem = contPrincipal.GetRegistro(regFuente1) + regDest;
                    int loadRes = LoadWord(regFuente2, posMem);

                    break;
                case 43:
                    /* *
                     * SW RX, n(rY)
                     * m(N+(RY)) = rX
                     * codOp: 51 RF1: Y RF2 O RD: X RD O IMM: n
                     * */
                    posMem = contPrincipal.GetRegistro(regFuente1) + regDest;
                    int storeRes = StoreWord(posMem, regFuente2);
                    break;
                case 50:
                    /* *
                     * LL Rx, n(Ry) Load conditional
                     * Rx <- M(n + (Ry))
                     * Rl <- n+(Ry)
                     * codOp: 50 RF1: Y RF2 O RD: X RD O IMM: n
                     * */
                    break;
                case 51:
                    /* *
                     * SC RX, n(rY) Store conditional
                     * IF (rl = N+(Ry)) => m(N+(RY)) = rX
                     * ELSE Rx =0
                     *  codOp: 51 RF1: Y RF2 O RD: X RD O IMM: n
                     * */
                    break;
                case 63:
                    /*
                     fin
                     CodOp: 63 RF1: 0 RF2 O RD: 0 RD o IMM:0
                     */
                    res = true;
                    break;
            }
            contPrincipal.AumentarPC(4);

            return res;
        }

        private int StoreWord(int posMem, int regFuente2)
        {
            throw new NotImplementedException();
        }

        private int LoadWord(int regFuente2, int posMem)
        {
            throw new NotImplementedException();
        }

        private void AvanzarReloj(int ciclos)
        {
            throw new NotImplementedException();
        }

        public int GetNumeroBloque(int direccion)
        {
            return (direccion / 16);
        }

        public int GetNumeroPalabra(int direccion, int tamannoCache)
        {
            int temp = direccion % 16;
            return (temp / tamannoCache);
        }

        public int GetPosicionCache(int numeroBloque)
        {
            return (numeroBloque % 4);
        }

        /*
         * DisminuirQuantum Disminuir el Quantum actual
         */
        public void DisminuirQuantum()
        {
            quantum--;
        }

        /*
         * AumentarPCContexto Aumentar el PC del Contexto
         * 
         * @param int Aumento del PC
         */
        public void AumentarPCContexto(int aumento = 4)
        {
            contextoEnEjecucion.AumentarPC(aumento);
        }

        /*
         * EncolarContexto Obtener el contexto actual para enviarlo a la cola de contextos
         */
        public void EncolarContexto()
        {
            colaContextos.Enqueue(contextoEnEjecucion);
        }

        /*
         * GetRegistro Obtiene el valor del registro reg
         * 
         * @param int Número de registro
         * @return int
         */
        public int GetRegistro(int reg)
        {
            return contextoEnEjecucion.GetRegistro(reg);
        }

        /*
         * SetRegistro Modifica el valor del registro reg, colocando nValor en él
         * 
         * @param int Número de registro
         * @param int valor que se coloca en el registro
         */
        public void SetRegistro(int reg, int nValor)
        {
            contextoEnEjecucion.SetRegistro(reg, nValor);
        }

        /*
         * SetPC Modifica el valor del PC
         * 
         * @param int valor a modificar
         */
        public void SetPC(int nPC)
        {
            contextoEnEjecucion.SetPC(nPC);
        }

        /*
         * GetPC Retorna el valor del PC
         * 
         * @return int
         */
        public int GetPC()
        {
            return contextoEnEjecucion.GetPC();
        }
    }
}
