using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Arqui_MIPS.CacheDatos;

namespace Arqui_MIPS
{
    class BusDatos
    {
        //Parámetros de la clase
        Memoria memoriaPrincipal;
        //Nucleo n0;
        //Nucleo n1;

        /*
         * Constructor de la clase
         */
        public BusDatos(Memoria memoria)
        {
            memoriaPrincipal = memoria;
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
                //n0.cacheDatos.SetBloque(iBloque,elBloque); //En la caché de datos, bloque iBloque, colocar elBloque
            }
            else
            {
                //n1.cacheDatos.SetBloque(iBloque,elBloque); //En la caché de datos, bloque iBloque, colocar elBloque              
            }
        }

        /*
         * Modificar estado de un bloque en la caché de un núcleo
         */
        public void CambiarEstadoBloqueCache(int nucleo, int iBloque, int nEstado)
        {
            if (nucleo == 0)
            {
                //n0.cacheDatos.SetEstadoBloque(iBloque,nEstado); //En la caché de datos, bloque iBloque, colocar el estado nEstado               
            }
            else
            {
                //n1.cacheDatos.SetEstadoBloque(iBloque,nEstado); //En la caché de datos, bloque iBloque, colocar el estado nEstado
            }
        }
        
        /*
         * Retorna el estado de un bloque en la caché de datos de un núcleo
         */
        public int GetEstadoBloqueCache(int nucleo, int iBloque)
        {
            int estado = 0;
            if (nucleo == 0)
            {
                //estado = n0.cacheDatos.GetEstadoBloque(iBloque); //En la caché de datos, retornar el estado del bloque iBloque
            }
            else
            {
                //estado = n1.cacheDatos.GetEstadoBloque(iBloque); //En la caché de datos, retornar el estado del bloque iBloque
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
                //etiqueta = n0.cacheDatos.GetEtiquetaBloque(iBloque); //En la caché de datos, retornar la etiqueta del bloque iBloque
            }
            else
            {
                //etiqueta = n1.cacheDatos.GetEtiquetaBloque(iBloque); //En la caché de datos, retornar la etiqueta del bloque iBloque
            }
            return etiqueta;
        }
    }
}
