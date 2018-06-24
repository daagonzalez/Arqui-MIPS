namespace Arqui_MIPS
{
    class Memoria
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

        public static string PrintInstrucciones()
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
        public int GetPalabraDato(int indMem, int palabra)
        {
            return memoriaDatos[indMem / 16].GetPalabra(palabra);
        }

        public int GetEstadoDato(int indMem)
        {
            return memoriaDatos[indMem / 16].GetEstado();
        }

        public int GetEtiquetaDato(int indMem)
        {
            return memoriaDatos[indMem / 16].GetEtiqueta();
        }

        //Memoria de instrucciones
        public int[] GetPalabraInstruccion(int indMem, int palabra)
        {
            return memoriaInstrucciones[(indMem / 16) - TAMANO_MEMORIA_DATOS].GetPalabra(palabra);
        }

        public int GetEtiquetaInstruccion(int indMem)
        {
            return memoriaInstrucciones[(indMem / 16) - TAMANO_MEMORIA_DATOS].GetEtiqueta();
        }

        /*
         * Setters
         */
         //Memoria de datos
        public void SetPalabraDato(int indMem, int palabra, int nValor)
        {
            memoriaDatos[indMem / 16].SetPalabra(nValor, palabra);
        }
        public void SetEstadoDato(int indMem, int nEstado)
        {
            memoriaDatos[indMem / 16].SetEstado(nEstado);
        }
        public void SetEtiquetaDato(int indMem, int nEtiqueta)
        {
            memoriaDatos[indMem / 16].SetEtiqueta(nEtiqueta);
        }

        //Memoria de instrucciones
        /*
         *@return bool true si se pudo guardar la palabra y false si no. 
         */
        public bool SetPalabraInstruccion(int indMem, int palabra, int[] nValor)
        {
            if ((indMem / 16) - TAMANO_MEMORIA_DATOS >= TAMANO_MEMORIA_INSTRUCCIONES)
            {
                return false;
            }
            memoriaInstrucciones[(indMem / 16) - TAMANO_MEMORIA_DATOS].SetPalabra(nValor, palabra);
            return true;
        }

        public void SetEtiquetaInstruccion(int indMem, int nEtiqueta)
        {
            memoriaInstrucciones[(indMem / 16) - TAMANO_MEMORIA_DATOS].SetEtiqueta(nEtiqueta);
        }
    }

    public class BloqueDato
    {
        int etiqueta;
        int estado;
        int[] palabras;

        public BloqueDato(int etiqueta)
        {
            this.etiqueta = etiqueta;
            estado = -1;
            palabras = new int[4];
            for (int i = 0; i < 4; i++)
                palabras[i] = 1;
        }
        public void SetEtiqueta(int nEtiqueta)
        {
            etiqueta = nEtiqueta;
        }
        public void SetEstado(int nEstado)
        {
            estado = nEstado;
        }
        public void SetPalabra(int nPalabra, int i)
        {
            palabras[i] = nPalabra;
        }

        public int GetEtiqueta()
        {
            return etiqueta;
        }
        public int GetEstado()
        {
            return estado;
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
