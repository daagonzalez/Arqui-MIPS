using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arqui_MIPS
{
    class CacheDatos
    {
        private const int CANTIDAD_BLOQUES = 8; //Corroborar
        BloqueCacheDatos[] bloques;

        /*
         * Constructor de la clase
         */
        public CacheDatos()
        {
            bloques = new BloqueCacheDatos[CANTIDAD_BLOQUES];
            for (int i = 0; i < CANTIDAD_BLOQUES; i++)
            {
                bloques[i] = new BloqueCacheDatos();
            }
        }

        /*
         * SetBloque Colocar elBloque en la caché de datos, bloque iBloque
         * 
         * @param int Indice del arreglo de bloques que se va a modificar
         * @param BloqueCacheDatos Bloque que se va a insertar
         */ 
        public void SetBloque(int iBloque, BloqueCacheDatos elBloque)
        {
            bloques[iBloque] = elBloque;
        }

        /*
         * SetEstadoBloque Colocar el estado nEstado en la caché de datos, bloque iBloque
         * 
         * @param int Indice del arreglo de bloques cuyo estado se va a modificar
         * @param BloqueCacheDatos.Estado nuevo estado del bloque
         */
        public void SetEstadoBloque(int iBloque, BloqueCacheDatos.Estado nEstado)
        {
            bloques[iBloque].SetEstado(nEstado);
        }

        /*
         * GetEstadoBloque En la caché de datos, retornar el estado del bloque iBloque
         * 
         * @param int Indice del arreglo de bloque cuyo estado se va a consultar
         * @return BloqueCacheDatos.Estado
         */
        public BloqueCacheDatos.Estado GetEstadoBloque(int iBloque)
        {
            return bloques[iBloque].GetEstado();
        }

        /*
         * GetEtiquetaBloque En la caché de datos, retornar la etiqueta del bloque iBloque
         * 
         * @param int Indice del arreglo de bloques cuya etiqueta se va a consultar
         * @return int
         */
         public int GetEtiquetaBloque(int iBloque)
        {
            return bloques[iBloque].GetEtiqueta();
        }


        /**
         * Clase para los Bloques
         **/
        public class BloqueCacheDatos
        {
            int[] palabras;
            Estado estado;
            int etiqueta;

            public enum Estado {I, C, M}

            public BloqueCacheDatos()
            {
                palabras = new int[4];
                estado = Estado.I;
                etiqueta = -1;
            }

            public int GetPalabra(int iPalabra)
            {
                return palabras[iPalabra];
            }

            public Estado GetEstado()
            {
                return estado;
            }

            public int GetEtiqueta()
            {
                return etiqueta;
            }

            public void SetPalabra(int iPalabra, int nPalabra)
            {
                palabras[iPalabra] = nPalabra;
            }

            public void SetEstado(Estado nEstado)
            {
                estado = nEstado;
            }

            public void SetEtiqueta(int nEtiqueta)
            {
                etiqueta = nEtiqueta;
            }
        }
    }
}
