namespace Arqui_MIPS
{
    public class Memoria
    {
        //Constantes
        public const int TAMANO_MEMORIA_DATOS = 24;
        public const int TAMANO_MEMORIA_INSTRUCCIONES = 40;

        //Arreglos de bloques
        public static BloqueDato[] memoriaDatos;
        public static BloqueInstruccion[] memoriaInstrucciones;

        /*
         * Constructor de la clase
         */
        public Memoria()
        {
            memoriaDatos = new BloqueDato[TAMANO_MEMORIA_DATOS];
            memoriaInstrucciones = new BloqueInstruccion[TAMANO_MEMORIA_INSTRUCCIONES];

            //Inicializar etiquetas
            int et = 0;
            for (int i = 0; i < (TAMANO_MEMORIA_DATOS); i++)
            {
                memoriaDatos[i] = new BloqueDato(et);
                et++;
            }
            for (int i = 0; i < (TAMANO_MEMORIA_INSTRUCCIONES); i++)
            {
                memoriaInstrucciones[i] = new BloqueInstruccion(et);
                et++;
            }
        }

        public string Print() //WIP
        {
            string res = "";
            foreach (BloqueInstruccion bi in memoriaInstrucciones)
            {
                for (int i = 0; i < 4; i++)
                {
                    int[] palabra = bi.GetPalabra(i);
                    res += palabra[0] + "-" + palabra[1] + "-" + palabra[2] + "-" + palabra[3] + "\n";
                }
            }
            return res;
        }

        /*
         * Getters
         */
        //Memoria de datos
        public int GetPalabraDato(int bloque, int palabra)
        {
            return memoriaDatos[bloque].GetPalabra(palabra);
        }

        public int GetEtiquetaDato(int bloque)
        {
            return memoriaDatos[bloque].GetEtiqueta();
        }

        //Memoria de instrucciones
        public int[] GetPalabraInstruccion(int bloque, int palabra)
        {
            return memoriaInstrucciones[(bloque) - TAMANO_MEMORIA_DATOS].GetPalabra(palabra);
        }

        public int GetEtiquetaInstruccion(int bloque)
        {
            return memoriaInstrucciones[(bloque) - TAMANO_MEMORIA_DATOS].GetEtiqueta();
        }

        
        /*
         * Setters
         */
        //Memoria de datos
        public void SetPalabraDato(int bloque, int palabra, int nValor)
        {
            memoriaDatos[bloque].SetPalabra(nValor, palabra);
        }

        public void SetEtiquetaDato(int bloque, int nEtiqueta)
        {
            memoriaDatos[bloque].SetEtiqueta(nEtiqueta);
        }

        //Memoria de instrucciones
        /*
         *@return bool true si se pudo guardar la palabra y false si no. 
         */
        public bool SetPalabraInstruccion(int bloque, int palabra, int[] nValor)
        {
            if ((bloque) - TAMANO_MEMORIA_DATOS >= TAMANO_MEMORIA_INSTRUCCIONES)
            {
                return false;
            }
            memoriaInstrucciones[(bloque) - TAMANO_MEMORIA_DATOS].SetPalabra(nValor, palabra);
            return true;
        }

        public void SetEtiquetaInstruccion(int bloque, int nEtiqueta)
        {
            memoriaInstrucciones[(bloque) - TAMANO_MEMORIA_DATOS].SetEtiqueta(nEtiqueta);
        }
    }

    public class BloqueDato
    {
        int etiqueta;
        int[] palabras;

        public BloqueDato(int etiqueta)
        {
            this.etiqueta = etiqueta;
            palabras = new int[4];
            for (int i = 0; i < 4; i++)
                palabras[i] = 1;
        }

        public void SetEtiqueta(int nEtiqueta)
        {
            etiqueta = nEtiqueta;
        }
        
        public void SetPalabra(int nPalabra, int i)
        {
            palabras[i] = nPalabra;
        }

        public int GetEtiqueta()
        {
            return etiqueta;
        }
        
        public int GetPalabra(int i)
        {
            return palabras[i];
        }
    }

    public class BloqueInstruccion
    {
        int etiqueta;
        int[][] palabras;

        public BloqueInstruccion(int etiqueta)
        {
            this.etiqueta = etiqueta;
            palabras = new int[4][];
            for (int i = 0; i < 4; i++)
            {
                palabras[i] = new int[4];
                for (int j = 0; j < 4; j++)
                    palabras[i][j] = 1;
            }
        }

        public void SetEtiqueta(int nEtiqueta)
        {
            etiqueta = nEtiqueta;
        }
        public void SetPalabra(int[] nPalabra, int i)
        {
            palabras[i] = nPalabra;
        }

        public int GetEtiqueta()
        {
            return etiqueta;
        }
        public int[] GetPalabra(int i)
        {
            return palabras[i];
        }
    }
}
