using System.Collections.Generic;

namespace Arqui_MIPS
{
    public class Nucleo
    {
        Contexto contextoEnEjecucion;
        BusDatos busDatos;
        BusInstrucciones busInstrucciones;
        CacheDatos cacheDatos;
        CacheInstrucciones cacheInstrucciones;
        int identificador;
        int quantum;
        Queue<Contexto> colaContextos;
        List<Contexto> contextosTerminados;

        /*
         * Constructor de la clase
         */
        public Nucleo(BusDatos bd, BusInstrucciones bi, int id, int quantumInicial, ref Queue<Contexto> colaContextos, ref List<Contexto> contextosTerminados)
        {
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
