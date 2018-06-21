using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arqui_MIPS
{
    public partial class Simulacion : Form
    {
        //Constantes
        private const int DIRECCION_INICIO_INSTRUCCION = 384;

        //Memoria
        Memoria memoria;

        //Contextos
        Queue<Contexto> colaContextos;
        List<Contexto> contextosTerminados;

        //Parametros de inicio
        private List<string> lineasHilillos;
        private int quantum;
        private bool ejecucionLenta;

        public Simulacion(List<string> lineas, int quantumIngresado, bool lenta)
        {
            InitializeComponent();

            memoria = new Memoria();
            colaContextos = new Queue<Contexto>();
            contextosTerminados = new List<Contexto>();

            lineasHilillos = lineas;
            quantum = quantumIngresado;
            ejecucionLenta = lenta;
            string msg = "";
            foreach (string linea in lineas)
            {
                msg += linea + "\n";
            }
            MessageBox.Show(msg);
        }

        public void CargarInstrucciones()
        {
            int indiceInstruccion = DIRECCION_INICIO_INSTRUCCION;
            int indicePalabra = 0;
            foreach (string linea in lineasHilillos)
            {
                if (indicePalabra >= 4)
                    indicePalabra = 0;
                string[] partesLinea = linea.Split(' ');
                int codigoOperacion = Int32.Parse(partesLinea[0]);
                int rX = Int32.Parse(partesLinea[2]);
                int rY = Int32.Parse(partesLinea[1]);
                int rZ = Int32.Parse(partesLinea[3]);

                int[] palabra = { codigoOperacion, rX, rY, rZ };
                memoria.SetPalabraInstruccion(indiceInstruccion, indicePalabra, palabra);
                indiceInstruccion++;
                indicePalabra++;
            }
        }
    }
}
