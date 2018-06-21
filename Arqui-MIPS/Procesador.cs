using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arqui_MIPS
{
    class Procesador
    {
        //Constantes
        public const int tamanoMemoriaDatos = 24;
        public const int tamanoMemoriaInstrucciones = 40;

        //Memoria
        public BloqueDato[] memoriaDatos;
        public BloqueInstruccion[] memoriaInstrucciones;
        public Queue<Contexto> colaContextos;
        public List<Contexto> contextosTerminados;

        /*
        * Constructor de la clase
        */
        public Procesador()
        {
            memoriaDatos = new BloqueDato[tamanoMemoriaDatos];
            memoriaInstrucciones = new BloqueInstruccion[tamanoMemoriaInstrucciones];

            //Inicializar etiquetas
            int et = 0;
            for (int i = 0; i < (tamanoMemoriaDatos); i++)
            {
                memoriaDatos[i].setEtiqueta(et);
                et++;
            }
            for (int i = 0; i < (tamanoMemoriaInstrucciones); i++)
            {
                memoriaInstrucciones[i].setEtiqueta(et);
                et++;
            }
        }


        public class BloqueDato
        {
            int etiqueta;
            int estado;
            int[] palabras;

            public BloqueDato()
            {
                etiqueta = -1;
                estado = -1;
                palabras = new int[4];
            }
            public void setEtiqueta(int nEtiqueta)
            {
                etiqueta = nEtiqueta;
            }
            public void setEstado(int nEstado)
            {
                estado = nEstado;
            }
            public void setPalabra(int nPalabra, int i)
            {
                palabras[i] = nPalabra;
            }

            public int getEtiqueta()
            {
                return etiqueta;
            }
            public int getEstado()
            {
                return estado;
            }
            public int getPalabra(int i)
            {
                return palabras[i];
            }
        }

        public class BloqueInstruccion
        {
            int etiqueta;
            int[][] palabras;

            public BloqueInstruccion()
            {
                etiqueta = -1;
                for (int i = 0; i < 4; i++)
                {
                    palabras[i] = new int[4];
                }
            }
            
            public void setEtiqueta(int nEtiqueta)
            {
                etiqueta = nEtiqueta;
            }
            public void setPalabra(int[] nPalabra, int i)
            {
                palabras[i] = nPalabra;
            }

            public int getEtiqueta()
            {
                return etiqueta;
            }
            public int[] getPalabra(int i)
            {
                return palabras[i];
            }
        }
    }
}
