using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Arqui_MIPS.CacheDatos;

namespace Arqui_MIPS
{
    public partial class Resultados : Form
    {
        List<Contexto> contextosTerminados;
        Nucleo n0;
        Nucleo n1;
        Memoria memoria;

        public Resultados(List<Contexto> listaContextos, Nucleo n0, Nucleo n1, Memoria memoria)
        {
            InitializeComponent();
            contextosTerminados = listaContextos;
            this.n0 = n0;
            this.n1 = n1;
            this.memoria = memoria;
        }

        private void btnFin_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Resultados_Load(object sender, EventArgs e)
        {
            //Cargar los hilillos en la combobox de hilillos
            foreach (Contexto contexto in contextosTerminados)
            {
                cbHilillos.Items.Add("Contexto " + contexto.GetId());
            }
            cbHilillos.SelectedIndex = 0;

            //Cargar los datos de la caché de datos
            var CacheN0 = n0.GetCacheDatos();
            var CacheN1 = n1.GetCacheDatos();
            CargarDatosCache(CacheN0, 0);
            CargarDatosCache(CacheN1, 1);

            //Cargar la memoria
            tbMemoria.Text = memoria.Print();
        }

        private void CargarDatosCache(CacheDatos cache, int indiceCache)
        {
            var txtEtiquetas = "";
            var txtValores = "";
            var txtEstados = "";

            for (int i = 0; i < CacheDatos.CANTIDAD_BLOQUES; i++)
            {
                txtEtiquetas += cache.GetEtiquetaBloque(i);
                if (i < CacheDatos.CANTIDAD_BLOQUES - 1)
                {
                    txtEtiquetas += " | ";
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < CacheDatos.CANTIDAD_BLOQUES; j++)
                {
                    txtValores += cache.GetPalabraBloque(i, j);
                    if (i < CacheDatos.CANTIDAD_BLOQUES - 1)
                    {
                        txtValores += " | ";
                    }
                    else
                    {
                        txtValores += "\n";
                    }
                }

            }

            for (int i = 0; i < CacheDatos.CANTIDAD_BLOQUES; i++)
            {
                txtEstados += cache.GetEstadoBloque(i);
                if (i < CacheDatos.CANTIDAD_BLOQUES - 1)
                {
                    txtEstados += " | ";
                }
            }

            if (indiceCache == 0)
            {
                tbCache0Estados.Text = txtEstados;
                tbCache0Valores.Text = txtValores;
                tbCache0Etiquetas.Text = txtEtiquetas;
            }
            else
            {
                tbCache1Estados.Text = txtEstados;
                tbCache1Valores.Text = txtValores;
                tbCache1Etiquetas.Text = txtEtiquetas;
            }
        }

        private void cbHilillos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbHilillos.SelectedIndex > 0)
            {
                lblCiclos.Text = contextosTerminados[cbHilillos.SelectedIndex - 1].GetDuracion().ToString();
                lbRegistros.Items.Clear();
                for (int i = 0; i < 32; i++)
                {
                    lbRegistros.Items.Add("Reg " + i + " " + contextosTerminados[cbHilillos.SelectedIndex - 1].GetRegistro(i));
                }
            }
        }
    }
}
