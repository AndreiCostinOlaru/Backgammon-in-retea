using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tablete
{
    class Zar
    {
        private Random zar;
        private int valoareZar;
        private int idZar;
        private Button butonZar;
        private fJoc f;
        private StreamWriter scriere;
        public Zar(fJoc form, int id)
        {
            f = form;
            idZar = id;
        }
        public void SetCell(int column, int row)
        {
            butonZar = new Button();
            butonZar.BackColor = System.Drawing.Color.Transparent;
            butonZar.Visible = false;
            butonZar.BackgroundImageLayout = ImageLayout.Zoom;
            butonZar.Click += butonZar_Click;
            butonZar.Dock = System.Windows.Forms.DockStyle.Fill;
            butonZar.Parent = f.GetTableLayoutPanel();
            butonZar.FlatStyle = FlatStyle.Flat;
            f.GetTableLayoutPanel().Controls.Add(butonZar, column, row);
        }
        public int Roll(int roll = 0, bool dubla = false)
        {
            zar = new Random();
            valoareZar = zar.Next(1, 7);
            if (dubla)
            {
                valoareZar = roll;
            }
            switch (valoareZar)
            {
                case 1:
                    butonZar.BackgroundImage = Properties.Resources._1;
                    break;
                case 2:
                    butonZar.BackgroundImage = Properties.Resources._2;
                    break;
                case 3:
                    butonZar.BackgroundImage = Properties.Resources._3;
                    break;
                case 4:
                    butonZar.BackgroundImage = Properties.Resources._4;
                    break;
                case 5:
                    butonZar.BackgroundImage = Properties.Resources._5;
                    break;
                case 6:
                    butonZar.BackgroundImage = Properties.Resources._6;
                    break;
            }
            return valoareZar;
        }
        private void butonZar_Click(object sender, EventArgs e)
        {
            scriere = new StreamWriter(f.GetNetworkStream());
            scriere.AutoFlush = true;
            if (f.GetJoc().GetValoareZarClick() == 0)
            {
                f.GetJoc().SetValoareZarClick(valoareZar);
                scriere.WriteLine("zar" + "*" + idZar + "*" + valoareZar);
                SetInvisibileButonZar();

            }
        }
        public void SetVisibileButonZar()
        {
            butonZar.Visible = true;
        }
        public void SetInvisibileButonZar()
        {
            butonZar.Visible = false;
        }
        public bool GetVisibileButonZar()
        {
            return butonZar.Visible;
        }
    }
}
