namespace Arqui_MIPS
{
    class Nucleo
    {
        Contexto contextoEnEjecucion;
        BusDatos busDatos;
        BusInstrucciones busInstrucciones;
        CacheDatos cacheDatos;
        CacheInstrucciones cacheInstrucciones;
        int identificador;

        /*
         * Constructor de la clase
         */
        public Nucleo(BusDatos bd, BusInstrucciones bi, int id)
        {
            busDatos = bd;
            busInstrucciones = bi;
            identificador = id;

            cacheDatos = new CacheDatos();
            cacheInstrucciones = new CacheInstrucciones();
        }

        /*
         * SetContextoEnEjecucion Recibe el contexto que se va a ejecutar y lo almacena en la variable respectiva
         * 
         * @param Contexto contexto a ejecutar
         */
        public void SetContextoEnEjecucion(Contexto elContexto)
        {
            contextoEnEjecucion = elContexto;
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

        /*
         * DisminuirQuantum Disminuir Quantum del Contexto
         */ 
        public void DisminuirQuantum()
        {
            contextoEnEjecucion.DisminuirQuantum();
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
         * GetContextoEnEjecucion Obtener el contexto actual para enviarlo a la cola de contextos
         */
        public Contexto GetContextoEnEjecucion()
        {
            contextoEnEjecucion.ResetQuantum();
            return contextoEnEjecucion;      
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
