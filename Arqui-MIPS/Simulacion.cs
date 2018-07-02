using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Arqui_MIPS
{
    public partial class Simulacion : Form
    {
        //Constantes
        private const int DIRECCION_INICIO_INSTRUCCION = 384;
        private const int CANTIDAD_NUCLEOS = 2;

        //Memoria
        Memoria memoria;

        //Contextos
        Queue<Contexto> colaContextos;
        List<Contexto> contextosTerminados;

        //Parametros de inicio
        private List<List<string>> hilillos;
        private int quantum;
        private bool ejecucionLenta;

        //Nucleos
        Nucleo n0;
        Nucleo n1;

        //Buses
        BusDatos busDatos;
        BusInstrucciones busInstrucciones;

        //Reloj
        int reloj;

        public Simulacion(List<List<string>> hilillos, int quantumIngresado, bool esDespacio)
        {
            InitializeComponent();

            memoria = new Memoria();
            colaContextos = new Queue<Contexto>();
            contextosTerminados = new List<Contexto>();

            var sync = new Barrier(participantCount: CANTIDAD_NUCLEOS);     //Barrera para sincronizar

            this.hilillos = hilillos;
            quantum = quantumIngresado;
            ejecucionLenta = esDespacio;    // ejecución lenta = true; ejecución rápida = false.
            reloj = 0;
            CargarInstrucciones();

            busDatos = new BusDatos(memoria);
            busInstrucciones = new BusInstrucciones(memoria);
            
            n0 = new Nucleo(sync, busDatos, busInstrucciones, 0, quantumIngresado, ref colaContextos, ref contextosTerminados);
            n1 = new Nucleo(sync, busDatos, busInstrucciones, 1, quantumIngresado, ref colaContextos, ref contextosTerminados);

            var hiloCpu1 = new Thread(n0.Iniciar);
            var hiloCpu2 = new Thread(n1.Iniciar);

            hiloCpu1.Start();
            hiloCpu2.Start();

            hiloCpu1.Join();
            hiloCpu2.Join();
            //TEST
            Resultados fRes = new Resultados(contextosTerminados,n0,n1,memoria);
                fRes.Show();
            //TEST
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
                Contexto contexto = new Contexto(indiceInstruccion, idContexto, reloj); //Un contexto por hilillo
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
                    int bloqueDestino = memoria.GetNumeroBloque(indiceInstruccion);
                    if (!memoria.SetPalabraInstruccion(bloqueDestino, indicePalabra, palabra))
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
            if (ejecucionLenta)
            {
                if (e.KeyCode == Keys.Space)
                {
                    MessageBox.Show("Avanza 20 ciclos");
                }
            }
        }
    }
}
