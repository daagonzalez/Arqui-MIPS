using static Arqui_MIPS.CacheInstrucciones;

namespace Arqui_MIPS
{
    public class BusInstrucciones
    {
        //Parámetros de la clase
        Memoria memoriaPrincipal;
        Nucleo n0;
        Nucleo n1;

        /*
         * Constructor de la clase
         */
        public BusInstrucciones(Memoria memoria)
        {
            memoriaPrincipal = memoria;
        }

        /*
         * SetNucleos Setea las referencias a los nucleos
         */
        public void SetNucleos(Nucleo n0, Nucleo n1)
        {
            this.n0 = n0;
            this.n1 = n1;
        }

        /*
         * Escribir bloque de caché a memoria
         */
        public void BloqueAMem(BloqueCacheInstrucciones bloqueCache, int dirInicial)
        {
            for (int i = 0; i < 4; i++)
            {
                memoriaPrincipal.SetPalabraInstruccion(dirInicial, i, bloqueCache.GetPalabra(i));
            }
        }

        /*
         * Leer bloque de memoria
         */
        public BloqueCacheInstrucciones BloqueDeMem(int dirInicial)
        {
            BloqueCacheInstrucciones bloqueCache = new BloqueCacheInstrucciones();
            for (int i = 0; i < 4; i++)
            {
                bloqueCache.SetPalabra(i, memoriaPrincipal.GetPalabraInstruccion(dirInicial, i));
            }
            bloqueCache.SetEtiqueta(memoriaPrincipal.GetEtiquetaDato(dirInicial));
            return bloqueCache;
        }
        
        /*
         * Retorna la etiqueta de un bloque en la caché de datos de un núcleo
         */
        public int GetEtiquetaBloqueCache(int nucleo, int iBloque)
        {
            int etiqueta = 0;
            if (nucleo == 0)
            {
                etiqueta = n0.GetCacheInstrucciones().GetEtiquetaBloque(iBloque);
            }
            else
            {
                etiqueta = n1.GetCacheInstrucciones().GetEtiquetaBloque(iBloque);
            }
            return etiqueta;
        }
    }
}
