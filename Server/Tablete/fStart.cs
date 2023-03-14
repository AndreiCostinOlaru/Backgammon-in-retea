using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tablete
{
    public partial class fStart : Form
    {
        fReguli fReguli;
        fJoc fJoc;
        public fStart()
        {
            fReguli = new fReguli(this);
            InitializeComponent();
            fJoc = new fJoc();
        }

        private void butonStart_Click(object sender, EventArgs e)
        {
            this.Hide();
            fJoc.Show();
        }

        private void butonReguli_Click(object sender, EventArgs e)
        {
            this.Hide();
            fReguli.Show();
        }

        private void butonExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
