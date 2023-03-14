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
    public partial class fReguli : Form
    {
        fStart f;
        public fReguli(fStart fStart) { 
        
            f=fStart;
            InitializeComponent();
        }

        private void butonExit_Click(object sender, EventArgs e)
        {
            this.Hide();
            f.Show();
        }
    }
}
