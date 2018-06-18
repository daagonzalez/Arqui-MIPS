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
        public const int tamanoMemoriaDatos = 64;
        public const int tamanoMemoriaInstrucciones = 40;

        //Memoria
        public BloqueDato[] memoriaDatos;
        public BloqueInstruccion[] memoriaInstrucciones;

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
            int[] datos;

            public BloqueDato()
            {
                etiqueta = -1;
                estado = -1;
                datos = new int[4]; //TODO: Revisar
            }
            public void setEtiqueta(int nEtiqueta)
            {
                etiqueta = nEtiqueta;
            }
            public void setEstado(int nEstado)
            {
                estado = nEstado;
            }
            public void setDato(int nDato, int i)
            {
                datos[i] = nDato;
            }

            public int getEtiqueta()
            {
                return etiqueta;
            }
            public int getEstado()
            {
                return estado;
            }
            public int getDato(int i)
            {
                return datos[i];
            }
        }

        public class BloqueInstruccion
        {
            int etiqueta;
            int[] datos;

            public BloqueInstruccion()
            {
                etiqueta = -1;
                datos = new int[4]; //TODO: Revisar
            }
            
            public void setEtiqueta(int nEtiqueta)
            {
                etiqueta = nEtiqueta;
            }
            public void setDato(int nDato, int i)
            {
                datos[i] = nDato;
            }

            public int getEtiqueta()
            {
                return etiqueta;
            }
            public int getDato(int i)
            {
                return datos[i];
            }
        }
    }
}
