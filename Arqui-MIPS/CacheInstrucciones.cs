namespace Arqui_MIPS
{
    public class CacheInstrucciones
    {
        private const int CANTIDAD_BLOQUES = 4;
        BloqueCacheInstrucciones[] bloques;

        /*
         * Constructor de la clase
         */
        public CacheInstrucciones()
        {
            bloques = new BloqueCacheInstrucciones[CANTIDAD_BLOQUES];
            for (int i = 0; i < CANTIDAD_BLOQUES; i++)
            {
                bloques[i] = new BloqueCacheInstrucciones();
            }
        }

        /*
         * SetBloque Colocar elBloque en la caché de instrucciones
         * 
         * @param int Indice del arreglo de bloques que se va a modificar
         * @param BloqueCacheInstrucciones Bloque que se va a insertar
         */
        public void SetBloque(int iBloque, BloqueCacheInstrucciones elBloque)
        {
            bloques[iBloque] = elBloque;
        }
        
        /*
         * GetEtiquetaBloque En la caché de instrucciones, retornar la etiqueta del bloque iBloque
         * 
         * @param int Indice del arreglo de bloques cuya etiqueta se va a consultar
         * @return int
         */
        public int GetEtiquetaBloque(int iBloque)
        {
            return bloques[iBloque].GetEtiqueta();
        }

        /*
         * GetPalabraBloque En la caché de instrucciones, retornar la palabra iPalabra del bloque iBloque
         * 
         * @param int Indice del arreglo de bloques cuya etiqueta se va a consultar
         * @param int Indice de la palabra que se va a consultar
         * @return int[]
         */
        public int[] GetPalabraBloque(int iBloque, int iPalabra)
        {
            return bloques[iBloque].GetPalabra(iPalabra);
        }


        /**
         * Clase para los Bloques
         **/
        public class BloqueCacheInstrucciones
        {
            int[][] palabras;
            int etiqueta;

            public BloqueCacheInstrucciones()
            {
                palabras = new int[4][];
                for (int i = 0; i < 4; i++)
                {
                    palabras[i] = new int[4];
                }
                etiqueta = -1;
            }

            public int[] GetPalabra(int iPalabra)
            {
                return palabras[iPalabra];
            }
            
            public int GetEtiqueta()
            {
                return etiqueta;
            }

            public void SetPalabra(int iPalabra, int[] nPalabra)
            {
                palabras[iPalabra] = nPalabra;
            }

            public void SetEtiqueta(int nEtiqueta)
            {
                etiqueta = nEtiqueta;
            }
        }
    }
}
