using static Arqui_MIPS.CacheDatos;

namespace Arqui_MIPS
{
    class BusDatos
    {
        //Parámetros de la clase
        Memoria memoriaPrincipal;
        Nucleo n0;
        Nucleo n1;

        /*
         * Constructor de la clase
         */
        public BusDatos(Memoria memoria, Nucleo n0, Nucleo n1)
        {
            memoriaPrincipal = memoria;
            this.n0 = n0;
            this.n1 = n1;
        }

        /*
         * Escribir bloque de caché a memoria
         */
        public void BloqueAMem(BloqueCacheDatos bloqueCache, int dirInicial)
        {
            for (int i = 0; i < 4; i++)
            {
                memoriaPrincipal.SetPalabraDato(dirInicial, i, bloqueCache.GetPalabra(i));
            }
        }

        /*
         * Leer bloque de memoria
         */
        public BloqueCacheDatos BloqueDeMem(int dirInicial)
        {
            BloqueCacheDatos bloqueCache = new BloqueCacheDatos();
            for (int i = 0; i < 4; i++)
            {
                bloqueCache.SetPalabra(i, memoriaPrincipal.GetPalabraDato(dirInicial, i));
            }
            bloqueCache.SetEtiqueta(memoriaPrincipal.GetEtiquetaDato(dirInicial));
            bloqueCache.SetEstado(BloqueCacheDatos.Estado.C); //Creo que siempre se cargan compartidos
            return bloqueCache;
        }

        /*
         * Copiar Bloque de una caché a la otra
         */
        public void ModificarPalabraCache(int nucleoDestino, int iBloque, BloqueCacheDatos elBloque)
        {
            if (nucleoDestino == 0)
            {
                n0.GetCacheDatos().SetBloque(iBloque,elBloque);
            }
            else
            {
               n1.GetCacheDatos().SetBloque(iBloque,elBloque);
            }
        }

        /*
         * Modificar estado de un bloque en la caché de un núcleo
         */
        public void CambiarEstadoBloqueCache(int nucleo, int iBloque, BloqueCacheDatos.Estado nEstado)
        {
            if (nucleo == 0)
            {
                n0.GetCacheDatos().SetEstadoBloque(iBloque,nEstado);
            }
            else
            {
                n1.GetCacheDatos().SetEstadoBloque(iBloque,nEstado);
            }
        }

        /*
         * Retorna el estado de un bloque en la caché de datos de un núcleo
         */
        public BloqueCacheDatos.Estado GetEstadoBloqueCache(int nucleo, int iBloque)
        {
            BloqueCacheDatos.Estado estado = BloqueCacheDatos.Estado.I;
            if (nucleo == 0)
            {
                estado = n0.GetCacheDatos().GetEstadoBloque(iBloque);
            }
            else
            {
                estado = n1.GetCacheDatos().GetEstadoBloque(iBloque);
            }
            return estado;
        }

        /*
         * Retorna la etiqueta de un bloque en la caché de datos de un núcleo
         */
        public int GetEtiquetaBloqueCache(int nucleo, int iBloque)
        {
            int etiqueta = 0;
            if (nucleo == 0)
            {
                etiqueta = n0.GetCacheDatos().GetEtiquetaBloque(iBloque);
            }
            else
            {
                etiqueta = n1.GetCacheDatos().GetEtiquetaBloque(iBloque);
            }
            return etiqueta;
        }
    }
}