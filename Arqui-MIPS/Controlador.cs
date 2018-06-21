using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arqui_MIPS
{
    class Controlador
    {
        //Memoria
        public Memoria memoria;

        //Contextos
        public Queue<Contexto> colaContextos;
        public List<Contexto> contextosTerminados;

        //Parametros de inicio
        private string[] lineasHilillos;
        private int quantum;
        private bool ejecucionLenta;

        /*
        * Constructor de la clase
        */
        public Controlador()
        {
            
        }
        
    }
}
