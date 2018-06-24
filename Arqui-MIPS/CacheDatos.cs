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
