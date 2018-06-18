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
    public partial class Resultados : Form
    {
        public Resultados()
        {
            InitializeComponent();
        }

        private void btnFin_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
