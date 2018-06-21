using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arqui_MIPS
{
    class Memoria
    {
        //Constantes
        public const int tamanoMemoriaDatos = 24;
        public const int tamanoMemoriaInstrucciones = 40;

        //Arreglos de bloques
        public BloqueDato[] memoriaDatos;
        public BloqueInstruccion[] memoriaInstrucciones;

        /*
         * Constructor de la clase
         */
        public Memoria()
        {
            memoriaDatos = new BloqueDato[tamanoMemoriaDatos];
            memoriaInstrucciones = new BloqueInstruccion[tamanoMemoriaInstrucciones];

            //Inicializar etiquetas
            int et = 0;
            for (int i = 0; i < (tamanoMemoriaDatos); i++)
            {
                memoriaDatos[i] = new BloqueDato(et);
                et++;
            }
            for (int i = 0; i < (tamanoMemoriaInstrucciones); i++)
            {
                memoriaInstrucciones[i] = new BloqueInstruccion(et);
                et++;
            }
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
            return memoriaInstrucciones[indMem / 16].GetPalabra(palabra);
        }

        public int GetEtiquetaInstruccion(int indMem)
        {
            return memoriaInstrucciones[indMem / 16].GetEtiqueta();
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
        public void SetPalabraInstruccion(int indMem, int palabra, int[] nValor)
        {
            memoriaInstrucciones[indMem / 16].SetPalabra(nValor, palabra);
        }

        public void SetEtiquetaInstruccion(int indMem, int nEtiqueta)
        {
            memoriaInstrucciones[indMem/16].SetEtiqueta(nEtiqueta);
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
