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
        private List<List<string>> hilillos;
        private int quantum;
        private bool ejecucionLenta;

        public Simulacion(List<List<string>> hilillos, int quantumIngresado, bool lenta)
        {
            InitializeComponent();

            memoria = new Memoria();
            colaContextos = new Queue<Contexto>();
            contextosTerminados = new List<Contexto>();

            this.hilillos = hilillos;
            quantum = quantumIngresado;
            ejecucionLenta = lenta;
            CargarInstrucciones();
            Resultados fRes = new Resultados();
            fRes.Show();
        }

        /*
         * Carga las instrucciones en la memoria y crea los contextos para cada hilillo
         * Si no hay espacio suficiente, muestra un error y termina la ejecución de la simulación
         */
        public void CargarInstrucciones()
        {
            int indiceInstruccion = DIRECCION_INICIO_INSTRUCCION;
            int indicePalabra = 0;
            int idContexto = 0;
            foreach (List<string> lineasHilillos in hilillos)
            {
                //Crear el contexto y encolarlo en la cola de contextos
                Contexto contexto = new Contexto(indiceInstruccion, idContexto, 1, quantum); //Un contexto por hilillo
                colaContextos.Enqueue(contexto);
                idContexto++;

                //Parsear lineas para guardarlas en memoria
                foreach (string linea in lineasHilillos)
                {
                    if (indicePalabra >= 4)
                        indicePalabra = 0;

                    //Generar palabra para guardar en el bloque
                    string[] partesLinea = linea.Split(' ');
                    int codigoOperacion = Int32.Parse(partesLinea[0]);
                    int rX = Int32.Parse(partesLinea[2]);
                    int rY = Int32.Parse(partesLinea[1]);
                    int rZ = Int32.Parse(partesLinea[3]);

                    int[] palabra = { codigoOperacion, rX, rY, rZ };

                    //Guardar palabra
                    if (!memoria.SetPalabraInstruccion(indiceInstruccion, indicePalabra, palabra))
                    {
                        MessageBox.Show("No hay memoria suficiente para cargar el programa. Intente de nuevo con un programa más pequeño","Error cargando los hilillos", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        Application.Exit();
                    }

                    //Aumentar índices
                    indiceInstruccion += 4;
                    indicePalabra++;                    
                }                
            }
        }

        private void Simulacion_Load(object sender, EventArgs e)
        {
            if (ejecucionLenta)
            {
                lblModoLento.Show();
            }
        }

        private void Simulacion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (ejecucionLenta)
                {
                    MessageBox.Show("Avanza 20 ciclos");
                }
            }
        }
    }
}
