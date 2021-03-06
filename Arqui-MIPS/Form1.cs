﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arqui_MIPS
{
    public partial class FrmPrincipal : Form
    {
        List<List<string>> hilillos;
        public FrmPrincipal()
        {
            InitializeComponent();
            openFileDialog1.Title = "Seleccione los archivos con los hilillos";
            openFileDialog1.Filter = "Archivos .txt|*.txt";
            openFileDialog1.InitialDirectory = Application.StartupPath;
            hilillos = new List<List<string>>();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var fileSelected = openFileDialog1.ShowDialog();
            if (fileSelected == DialogResult.OK)
            {
                foreach (var fileName in openFileDialog1.FileNames)
                {
                    lvSelectedFiles.Items.Add(fileName);
                    List<string> lineas = new List<string>();
                    foreach (string linea in File.ReadAllLines(fileName))
                    {
                        lineas.Add(linea);
                    }
                    hilillos.Add(lineas);
                }
            }            
        }
        
        private void txtQuantum_TextChanged(object sender, EventArgs e)
        {
            if (lvSelectedFiles.Items.Count > 0 && txtQuantum.Text != "")
            {
                btnStart.Enabled = true;
            }
            else
            {
                btnStart.Enabled = false;
            }
        }

        private void txtQuantum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            int quantum = Int32.Parse(txtQuantum.Text);
            bool lenta = false;
            if (rdbLenta.Checked)
                lenta = true;

            Simulacion fSimulacion = new Simulacion(hilillos, quantum, lenta);
            fSimulacion.Show();
            this.Hide();
        }
    }
}
