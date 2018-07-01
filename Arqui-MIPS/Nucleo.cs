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
            lock (colaContextos)
            { //Se bloquea para que solo uno de los núcleos a la vez pueda sacar de la cola
                if (colaContextos.Count > 0)
                {
                    contextoEnEjecucion = colaContextos.Dequeue();
                }
                //Habría que hacer algo cuando ya no hay contextos para correr
            }

            while (true)
            {
                Run();
            }
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
                    AvanzarReloj(1);
                }
            }
            instruccion = cacheInstrucciones.GetPalabraBloque(nBloqueEnCache, nPalabra);
            EjecutarInstruccion(instruccion[0], instruccion[1], instruccion[2], instruccion[3]);
        }

        private void EjecutarInstruccion(int codigoOp, int rx, int ry, int rz)
        {
            switch (codigoOp)
            {
                case 8:
                    //DADDI
                    break;
                case 32:
                    //DADD
                    break;
                case 34:
                    //DSUB
                    break;
                case 12:
                    //DMUL
                    break;
                case 14:
                    //DDIV
                    break;
                case 4:
                    //BEQZ
                    break;
                case 5:
                    //BNEZ
                    break;
                case 3:
                    //JAL
                    break;
                case 2:
                    //JR
                    break;
                case 35:
                    //LW
                    break;
                case 43:
                    //SW
                    break;
                case 63:
                    //FIN
                    break;
            }
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
