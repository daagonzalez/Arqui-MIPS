using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Arqui_MIPS
{
    public class Nucleo
    {
        private const int CICLOS_TRAER_DE_MEMORIA = 40;
        private const int CICLOS_COPIAR_A_MEMORIA = 40;
        private const int TAMBLOQUE = 16;
        private const int TAMPALABRA = 4;

        Contexto contextoEnEjecucion;
        BusDatos busDatos;
        BusInstrucciones busInstrucciones;
        CacheDatos cacheDatos;
        CacheInstrucciones cacheInstrucciones;

        public List<Nucleo> nucleos { get; set; }       // // para revisar las otras caches

        // para sincronizacion
        public Barrier Sync;
        public int quantum;
        public int identificador, CicloActual, quantumInicial;
        public Queue<Contexto> colaContextos;
        public List<Contexto> contextosTerminados;

        /*
         * Constructor de la clase
         */
        public Nucleo(Barrier barrera, ref BusDatos bd, ref BusInstrucciones bi, int id, int quantumInicial, ref Queue<Contexto> colaContextos, ref List<Contexto> contextosTerminados)
        {
            Sync = barrera;
            busDatos = bd;
            busDatos.SetNucleo(id, this);
            busInstrucciones = bi;
            busInstrucciones.SetNucleo(id, this);
            identificador = id;
            quantum = quantumInicial;
            this.quantumInicial = quantumInicial;
            this.colaContextos = colaContextos;
            this.contextosTerminados = contextosTerminados;
            contextoEnEjecucion = null;

            nucleos = new List<Nucleo>();

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
            // mientras todavía existan contextos en la cola.      
            while (colaContextos.Count > 0)
            {
                if (contextoEnEjecucion == null)
                {
                    contextoEnEjecucion = colaContextos.Dequeue();
                }
                Run();
            }
            Sync.RemoveParticipant();
            //lock (colaContextos)
            // { //Se bloquea para que solo uno de los núcleos a la vez pueda sacar de la cola
            //    if (colaContextos.Count > 0)
            //    {
            //        contextoEnEjecucion = colaContextos.Dequeue();
            //    }
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
            int nPalabra = GetNumeroPalabra(pcContexto, TAMPALABRA);
            int nBloqueEnCache = GetPosicionCache(nBloque);

            //Revisar la caché de instrucciones
            bool falloCacheI = true;
            
            if (cacheInstrucciones.GetEtiquetaBloque(nBloqueEnCache) != nBloque)
            {
                /**FALLO DE CACHÉ**/
                while (falloCacheI)
                {
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
                            falloCacheI = false;
                        }
                    }
                    else
                    {
                        //No se pudo obtener el bus
                        AvanzarReloj(1);    // avanzar reloj es del hilo principal.
                    }
                }
            }
            instruccion = cacheInstrucciones.GetPalabraBloque(nBloqueEnCache, nPalabra);
            var res = ManejoInstrucciones(instruccion);
            if (res)
            {
                contextoEnEjecucion.SetCicloSalida(CicloActual);
                contextosTerminados.Add(contextoEnEjecucion);
                contextoEnEjecucion = null;
            }
            else
            {
                quantum--;
                if (quantum == 0)
                {
                    colaContextos.Enqueue(contextoEnEjecucion);
                    contextoEnEjecucion = null;
                    quantum = quantumInicial;
                }
            }
        }

        public bool ManejoInstrucciones(int[] instruccion)
        {
            bool res = false;

            int codigoInstruccion = instruccion[0],
                regFuente1 = instruccion[2],
                regFuente2 = instruccion[1],
                regDest = instruccion[3],
                posMem;

            //Contexto contPrincipal = colaContextos.Dequeue();
            string output = "";         //debug
            int pc = contextoEnEjecucion.GetPC();
            int nBloque = GetNumeroBloque(pc);
            int nPalabra = GetNumeroPalabra(pc, 4);

            switch (codigoInstruccion)
            {
                case 2:
                    /*
                    JR RX: PC=RX
                    CodOp: 2 RF1: X RF2 O RD: 0 RD o IMM:0
                    */
                    Console.WriteLine("(Hilillo " + contextoEnEjecucion.GetId() + ") Núcleo " + identificador + " ejecuta JR R" + regFuente1);
                    contextoEnEjecucion.SetPC(regFuente1);
                    AvanzarReloj(1);
                    break;
                case 3:
                    /*
                    JAL n, R31=PC, PC = PC+n
                    CodOp: 3 RF1: 0 RF2 O RD: 0 RD o IMM:n
                    */
                    Console.WriteLine("(Hilillo " + contextoEnEjecucion.GetId() + ") Núcleo " + identificador + " ejecuta JAL " + regDest);
                    contextoEnEjecucion.SetRegistro(31, pc);
                    contextoEnEjecucion.AumentarPC(regDest);
                    AvanzarReloj(1);
                    break;
                case 4:
                    /*
                    BEQZ RX, ETIQ : Si RX = 0 salta 
                    CodOp: 4 RF1: Y RF2 O RD: 0 RD o IMM:n
                    */
                    Console.WriteLine("(Hilillo " + contextoEnEjecucion.GetId() + ") Núcleo " + identificador + " ejecuta BEQZ R" + regFuente1 + ", " + regDest);
                    if (contextoEnEjecucion.GetRegistro(regFuente1) == 0)
                    {
                        //salta a la etiqueta indicada por regDest
                        contextoEnEjecucion.AumentarPC(regDest << 2);
                    }
                    AvanzarReloj(1);
                    break;
                case 5:
                    /*
                     BEQNZ RX, ETIQ : Si RX != 0 salta 
                     CodOp: 5 RF1: x RF2 O RD: 0 RD o IMM:n
                     */
                    Console.WriteLine("(Hilillo " + contextoEnEjecucion.GetId() + ") Núcleo " + identificador + " ejecuta BEQNZ R" + regFuente1 + ", " + regDest);
                    if (contextoEnEjecucion.GetRegistro(regFuente1) != 0)
                    {
                        //salta a la etiqueta indicada por regDest
                        contextoEnEjecucion.AumentarPC(regDest << 2);
                    }
                    AvanzarReloj(1);
                    break;
                case 8:
                    /*
                    DADDI RX, RY, #n : Rx <-- (Ry) + n
                    CodOp: 8 RF1: Y RF2 O RD: x RD O IMM:n
                    */
                    Console.WriteLine("(Hilillo " + contextoEnEjecucion.GetId() + ") Núcleo " + identificador + " ejecuta DADDI R" + regFuente2 + ", R" + regFuente1 + ", #" + regDest);
                    contextoEnEjecucion.SetRegistro(regFuente2, contextoEnEjecucion.GetRegistro(regFuente1) + regDest);
                    AvanzarReloj(1);
                    break;
                case 12:
                    /*
                    DMUL RX, RY, #n : Rx <-- (Ry) * (Rz)
                    CodOp: 12 RF1: Y RF2 O RD: z RD o IMM:X
                    */
                    Console.WriteLine("(Hilillo " + contextoEnEjecucion.GetId() + ") Núcleo " + identificador + " ejecuta DMUL R" + regDest + ", R" + regFuente1 + ", R" + regFuente2);
                    contextoEnEjecucion.SetRegistro(regDest, contextoEnEjecucion.GetRegistro(regFuente1) * contextoEnEjecucion.GetRegistro(regFuente2));
                    AvanzarReloj(1);
                    break;
                case 14:
                    /*
                    DDIV RX, RY, #n : Rx <-- (Ry) / (Rz)
                    CodOp: 14 RF1: Y RF2 O RD: z RD o IMM:X
                    */
                    Console.WriteLine("(Hilillo " + contextoEnEjecucion.GetId() + ") Núcleo " + identificador + " ejecuta DDIV R" + regDest + ", R" + regFuente1 + ", R" + regFuente2);
                    contextoEnEjecucion.SetRegistro(regDest, contextoEnEjecucion.GetRegistro(regFuente1) / contextoEnEjecucion.GetRegistro(regFuente2));
                    AvanzarReloj(1);
                    break;
                case 32:
                    /*
                    DADD RX, RY, #n : Rx <-- (Ry) + (Rz)
                    CodOp: 32 RF1: Y RF2 O RD: x RD o IMM:Rz
                    */
                    Console.WriteLine("(Hilillo " + contextoEnEjecucion.GetId() + ") Núcleo " + identificador + " ejecuta DADD R" + regDest + ", R" + regFuente1 + ", R" + regFuente2);
                    contextoEnEjecucion.SetRegistro(regDest, contextoEnEjecucion.GetRegistro(regFuente1) + contextoEnEjecucion.GetRegistro(regFuente2));
                    AvanzarReloj(1);
                    break;
                case 34:
                    /*
                    DSUB RX, RY, #n : Rx <-- (Ry) - (Rz)
                    CodOp: 34 RF1: Y RF2 O RD: z RD o IMM:X
                    */
                    Console.WriteLine("(Hilillo " + contextoEnEjecucion.GetId() + ") Núcleo " + identificador + " ejecuta DSUB R" + regDest + ", R" + regFuente1 + ", R" + regFuente2);
                    contextoEnEjecucion.SetRegistro(regDest, contextoEnEjecucion.GetRegistro(regFuente1) - contextoEnEjecucion.GetRegistro(regFuente2));
                    AvanzarReloj(1);
                    break;
                case 35:
                    /* *
                    * LW Rx, n(Ry)
                    * Rx <- M(n + (Ry))
                    * 
                    * codOp: 35 RF1: Y RF2 O RD: X RD O IMM: n
                    * */
                    Console.WriteLine("(Hilillo " + contextoEnEjecucion.GetId() + ") Núcleo " + identificador + " ejecuta LW R" + regFuente2 + ", " + regDest + "(R" + regFuente1 + ")");
                    posMem = contextoEnEjecucion.GetRegistro(regFuente1) + regDest;
                    int loadRes = LoadWord(regFuente2, posMem);
                    contextoEnEjecucion.SetRegistro(regFuente2, loadRes);
                    AvanzarReloj(1);
                    break;
                case 43:
                    /* *
                     * SW RX, n(rY)
                     * m(N+(RY)) = rX
                     * */
                    Console.WriteLine("(Hilillo " + contextoEnEjecucion.GetId() + ") Núcleo " + identificador + " ejecuta SW R" + regFuente2 + ", " + regDest + "(R" + regFuente1 + ")");
                    posMem = contextoEnEjecucion.GetRegistro(regFuente1) + regDest;
                    int storeRes = StoreWord(posMem, regFuente2);
                    AvanzarReloj(1);
                    break;
                case 50:
                    /* *
                     * LL Rx, n(Ry) Load conditional
                     * */
                    break;
                case 51:
                    /* *
                     * SC RX, n(rY) Store conditional
                     * */
                    break;
                case 63:
                    /*
                     fin
                     CodOp: 63 RF1: 0 RF2 O RD: 0 RD o IMM:0
                     */
                    Console.WriteLine("(Hilillo " + contextoEnEjecucion.GetId() + ") Núcleo " + identificador + " ejecuta FIN");
                    res = true;
                    AvanzarReloj(1);
                    break;
            }
            contextoEnEjecucion.AumentarPC(4);

            return res;
        }

        private int StoreWord(int posMem, int regFuente2)
        {
            int exito = 0;
            bool detenido;
            int nBloque = GetNumeroBloque(posMem);
            int nPalabra = GetNumeroPalabra(posMem, 4);
            int nBloqueEnCache = GetPosicionCache(nBloque);

            CacheDatos.BloqueCacheDatos elBloque = null;
            do
            {
                detenido = false;
                elBloque = cacheDatos.GetBloque(nBloqueEnCache);
                if (Monitor.TryEnter(cacheDatos.GetBloque(nBloqueEnCache)))
                {
                    try
                    {
                        if (elBloque.GetEtiqueta() == nBloque)
                        {
                            switch (elBloque.GetEstado())
                            {
                                case CacheDatos.BloqueCacheDatos.Estado.M:
                                    AvanzarReloj(1);        // Tiene el bloque en cache.
                                    break;
                                case CacheDatos.BloqueCacheDatos.Estado.C:      // tambien lo tiene pero debe revisar la otra cache para invalidar
                                    if (Monitor.TryEnter(busDatos))             // pide bus 
                                    {
                                        try
                                        {
                                            var otroNucleo = 0;
                                            if (identificador == 0)
                                            {
                                                otroNucleo = 1;
                                            }
                                            if (Monitor.TryEnter(busDatos.GetBloqueCache(otroNucleo, nBloqueEnCache)))
                                            {
                                                try
                                                {
                                                    var bloqueOtraCache = busDatos.GetBloqueCache(otroNucleo, nBloqueEnCache);
                                                    if (bloqueOtraCache.GetEtiqueta() == nBloque)
                                                    {
                                                        busDatos.CambiarEstadoBloqueCache(otroNucleo, nBloqueEnCache, CacheDatos.BloqueCacheDatos.Estado.I);
                                                    }
                                                }
                                                finally
                                                {
                                                    Monitor.Exit(busDatos.GetBloqueCache(otroNucleo, nBloqueEnCache));
                                                    AvanzarReloj(1);
                                                }
                                            }
                                            else
                                            {
                                                detenido = true;
                                            }
                                        }
                                        finally
                                        {
                                            Monitor.Exit(busDatos);
                                        }
                                    }
                                    else
                                    {                           // no logro el bus, debe retroceder el pc
                                        AvanzarReloj(1);        // espero 1
                                        detenido = true;
                                    }
                                    break;
                                case CacheDatos.BloqueCacheDatos.Estado.I:
                                    if (Monitor.TryEnter(busDatos))
                                    {
                                        try
                                        {
                                            var otroNucleo = 0;
                                            if (identificador == 0)
                                            {
                                                otroNucleo = 1;
                                            }
                                            if (Monitor.TryEnter(busDatos.GetBloqueCache(otroNucleo, nBloqueEnCache)))
                                            {
                                                try
                                                {
                                                    var bloqueOtraCache = busDatos.GetBloqueCache(otroNucleo, nBloqueEnCache);
                                                    if (bloqueOtraCache.GetEtiqueta() == nBloque)
                                                    {
                                                        switch (bloqueOtraCache.GetEstado())
                                                        {
                                                            case CacheDatos.BloqueCacheDatos.Estado.M:
                                                                busDatos.BloqueAMem(bloqueOtraCache, nBloque);
                                                                busDatos.CambiarEstadoBloqueCache(otroNucleo, nBloqueEnCache, CacheDatos.BloqueCacheDatos.Estado.I);
                                                                AvanzarReloj(CICLOS_COPIAR_A_MEMORIA);
                                                                break;
                                                            case CacheDatos.BloqueCacheDatos.Estado.C:
                                                                busDatos.CambiarEstadoBloqueCache(otroNucleo, nBloqueEnCache, CacheDatos.BloqueCacheDatos.Estado.I);
                                                                // Solo invalido.
                                                                break;
                                                        }
                                                    }
                                                }
                                                finally
                                                {
                                                    Monitor.Exit(busDatos.GetBloqueCache(otroNucleo, nBloqueEnCache));
                                                }
                                            }
                                            else
                                            {
                                                AvanzarReloj(1);
                                                detenido = true;
                                            }
                                        }
                                        finally
                                        {
                                            Monitor.Exit(busDatos);
                                        }
                                    }
                                    else
                                    {
                                        AvanzarReloj(1);
                                        detenido = true;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            // FALLO DE CACHE
                            if (elBloque.GetEstado() == CacheDatos.BloqueCacheDatos.Estado.M) // Debo guardar en memoria ese bloque
                            {
                                if (Monitor.TryEnter(busDatos))             // pide bus 
                                {
                                    try
                                    {
                                        busDatos.BloqueAMem(elBloque, elBloque.GetEtiqueta());
                                        busDatos.CambiarEstadoBloqueCache(identificador, nBloqueEnCache, CacheDatos.BloqueCacheDatos.Estado.C);
                                        AvanzarReloj(CICLOS_COPIAR_A_MEMORIA);
                                        var otroNucleo = 0;
                                        if (identificador == 0)
                                        {
                                            otroNucleo = 1;
                                        }
                                        if (Monitor.TryEnter(busDatos.GetBloqueCache(otroNucleo, nBloqueEnCache)))
                                        {
                                            try
                                            {
                                                var bloqueOtraCache = busDatos.GetBloqueCache(otroNucleo, nBloqueEnCache);
                                                if (bloqueOtraCache.GetEtiqueta() == nBloque)
                                                {
                                                    switch (bloqueOtraCache.GetEstado())
                                                    {
                                                        case CacheDatos.BloqueCacheDatos.Estado.M:
                                                            busDatos.BloqueAMem(bloqueOtraCache, nBloque);
                                                            busDatos.CambiarEstadoBloqueCache(otroNucleo, nBloqueEnCache, CacheDatos.BloqueCacheDatos.Estado.I);
                                                            AvanzarReloj(CICLOS_COPIAR_A_MEMORIA);
                                                            break;
                                                        case CacheDatos.BloqueCacheDatos.Estado.C:
                                                            busDatos.CambiarEstadoBloqueCache(otroNucleo, nBloqueEnCache, CacheDatos.BloqueCacheDatos.Estado.I);
                                                            // Solo invalido.
                                                            break;
                                                    }
                                                }
                                            }
                                            finally
                                            {
                                                Monitor.Exit(busDatos.GetBloqueCache(otroNucleo, nBloqueEnCache));
                                            }
                                        }
                                        else
                                        {
                                            AvanzarReloj(1);
                                            detenido = true;
                                        }

                                    }
                                    finally
                                    {
                                        Monitor.Exit(busDatos);
                                    }
                                }
                            }
                            // Cargo el bloque a cache para cambiarlo y dejar en estado modificado luego.
                            cacheDatos.SetBloque(nBloqueEnCache, busDatos.BloqueDeMem(nBloque));
                            elBloque = cacheDatos.GetBloque(nBloqueEnCache);          // aqui
                            elBloque.SetEstado(CacheDatos.BloqueCacheDatos.Estado.C);
                            //busDatos.CambiarEstadoBloqueCache(identificador, nBloqueEnCache, CacheDatos.BloqueCacheDatos.Estado.C);
                            
                            AvanzarReloj(CICLOS_TRAER_DE_MEMORIA);
                        }
                    }
                    finally
                    {
                        Monitor.Exit(cacheDatos.GetBloque(nBloqueEnCache));
                    }
                }
                else
                {
                    AvanzarReloj(1);
                    detenido = true;
                }
            } while (detenido);


            elBloque.SetPalabra(nPalabra, regFuente2);
            elBloque.SetEstado(CacheDatos.BloqueCacheDatos.Estado.M);
            cacheDatos.SetBloque(nBloqueEnCache, elBloque);

            return exito;
        }

        private int LoadWord(int regFuente2, int posMem)
        {
            int resultado = 0;
            bool detenido;
            int nBloque = GetNumeroBloque(posMem);
            int nPalabra = GetNumeroPalabra(posMem, 4);
            int nBloqueEnCache = GetPosicionCache(nBloque);

            CacheDatos.BloqueCacheDatos elBloque;
            do
            {
                detenido = false;
                if (Monitor.TryEnter(cacheDatos.GetBloque(nBloqueEnCache)))
                {
                    try
                    {
                        elBloque = cacheDatos.GetBloque(nBloqueEnCache);
                        if (elBloque.GetEtiqueta() == nBloque)
                        {
                            if (elBloque.GetEstado() == CacheDatos.BloqueCacheDatos.Estado.C || elBloque.GetEstado() == CacheDatos.BloqueCacheDatos.Estado.M)
                            {
                                AvanzarReloj(1);                // Bloque esta en la cache, todo bien
                            }
                            else
                            {
                                //El bloque está Inválido
                                if (Monitor.TryEnter(busDatos))
                                {
                                    try
                                    {
                                        CacheDatos.BloqueCacheDatos bloqueOtraCache;
                                        var otroNucleo = 0;
                                        if (identificador == 0)
                                        {
                                            otroNucleo = 1;
                                        }

                                        if (Monitor.TryEnter(busDatos.GetBloqueCache(otroNucleo, nBloqueEnCache)))
                                        {
                                            try
                                            {
                                                bloqueOtraCache = busDatos.GetBloqueCache(otroNucleo, nBloqueEnCache);
                                                if (bloqueOtraCache.GetEtiqueta() == nBloque)
                                                {
                                                    if (bloqueOtraCache.GetEstado() == CacheDatos.BloqueCacheDatos.Estado.M)
                                                    {
                                                        busDatos.BloqueAMem(bloqueOtraCache, nBloque);
                                                        busDatos.CambiarEstadoBloqueCache(otroNucleo, nBloqueEnCache, CacheDatos.BloqueCacheDatos.Estado.C);
                                                        cacheDatos.SetBloque(nBloqueEnCache, bloqueOtraCache);
                                                        AvanzarReloj(CICLOS_COPIAR_A_MEMORIA);
                                                        AvanzarReloj(1);
                                                    }
                                                }
                                                else
                                                {
                                                    cacheDatos.SetBloque(nBloqueEnCache, busDatos.BloqueDeMem(nBloque));
                                                    AvanzarReloj(CICLOS_TRAER_DE_MEMORIA);
                                                    AvanzarReloj(1);
                                                }
                                            }
                                            finally
                                            {
                                                Monitor.Exit(busDatos.GetBloqueCache(otroNucleo, nBloqueEnCache));
                                            }
                                        }
                                    }

                                    finally
                                    {
                                        Monitor.Exit(busDatos);
                                    }
                                }
                                else
                                {
                                    //Detenido en ejecución con IR listo
                                    detenido = true;
                                    AvanzarReloj(1);
                                }
                            }
                        }
                        else
                        {
                            // Reviso si mi bloque esta modificado, el que esta en la cache, de estarlo debo guardarlo en memoria. Hay un FALLO DE CACHE
                            if (Monitor.TryEnter(busDatos))
                            {
                                try
                                {
                                    if (elBloque.GetEstado() == CacheDatos.BloqueCacheDatos.Estado.M)   // bloque victima
                                    {
                                        busDatos.BloqueAMem(elBloque, elBloque.GetEtiqueta());
                                        busDatos.CambiarEstadoBloqueCache(identificador, nBloqueEnCache, CacheDatos.BloqueCacheDatos.Estado.C);
                                        AvanzarReloj(CICLOS_COPIAR_A_MEMORIA);
                                    }
                                    else
                                    {   // revisa la otra cache para ver si lo encuentra alla.
                                        CacheDatos.BloqueCacheDatos bloqueOtraCache;
                                        var otroNucleo = 0;
                                        if (identificador == 0)
                                        {
                                            otroNucleo = 1;
                                        }

                                        if (Monitor.TryEnter(busDatos.GetBloqueCache(otroNucleo, nBloqueEnCache)))
                                        {
                                            try
                                            {
                                                bloqueOtraCache = busDatos.GetBloqueCache(otroNucleo, nBloqueEnCache);
                                                if (bloqueOtraCache.GetEtiqueta() == nBloque)
                                                {
                                                    if (bloqueOtraCache.GetEstado() == CacheDatos.BloqueCacheDatos.Estado.M)
                                                    {
                                                        busDatos.BloqueAMem(bloqueOtraCache, nBloque);
                                                        busDatos.CambiarEstadoBloqueCache(otroNucleo, nBloqueEnCache, CacheDatos.BloqueCacheDatos.Estado.C);
                                                        cacheDatos.SetBloque(nBloqueEnCache, bloqueOtraCache);      // copia de la otra cache a la de el.
                                                        AvanzarReloj(CICLOS_COPIAR_A_MEMORIA);
                                                        AvanzarReloj(1);
                                                    }
                                                }
                                                else
                                                {
                                                    cacheDatos.SetBloque(nBloqueEnCache, busDatos.BloqueDeMem(nBloque));
                                                    AvanzarReloj(CICLOS_TRAER_DE_MEMORIA);
                                                    AvanzarReloj(1);
                                                }
                                            }
                                            finally
                                            {
                                                Monitor.Exit(busDatos.GetBloqueCache(otroNucleo, nBloqueEnCache));
                                            }
                                        }
                                    }
                                }

                                finally
                                {
                                    Monitor.Exit(busDatos);
                                }

                            }

                        }
                    }

                    finally
                    {
                        //Libera la posición de caché
                        Monitor.Exit(cacheDatos.GetBloque(nBloqueEnCache));
                    }
                }
                else
                {
                    AvanzarReloj(1);
                    detenido = true;
                }
            } while (detenido);

            resultado = cacheDatos.GetPalabraBloque(nBloqueEnCache, nPalabra);

            return resultado;
        }

        private void AvanzarReloj(int ciclos)
        {
            for (int i = 0; i < ciclos; i++)
            {
                Sync.SignalAndWait();
            }
            //Un ciclo for de 0 a 'ciclos', donde cada iteración espera en la barrera para indicarle al hilo principal que le sume 1 al reloj
        }

        public int GetNumeroBloque(int direccion)
        {
            return (direccion / TAMBLOQUE);
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
        public void AumentarPCContexto(int aumento)
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
