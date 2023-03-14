using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tablete
{
    abstract class Piesa : Triunghi
    {
        protected int id;
        protected PictureBox piesaImg;
        protected fJoc f;

        protected Piesa()
        {
            piesaImg = new PictureBox();
            piesaImg.BackColor = System.Drawing.Color.Transparent;
            piesaImg.Dock = System.Windows.Forms.DockStyle.Fill;
            piesaImg.SizeMode = PictureBoxSizeMode.StretchImage;
            piesaImg.MouseClick += piesaImg_MouseClick;
        }
        public abstract bool Muta(int valoareZar);
        public abstract bool IsTop();
        public abstract bool ToateInCasa();
        public bool VerificaDacaInlaturat()
        {
            TableLayoutPanelCellPosition pozitieAux = f.GetTableLayoutPanel().GetCellPosition(piesaImg);
            TableLayoutPanelCellPosition pozitieCentru = new TableLayoutPanelCellPosition(7, 7);
            if (pozitieAux.Equals(pozitieCentru)) return true;
            return false;
        }
        private void piesaImg_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("ID: " + id);
            f.GetJoc().SetIdClick(id);
        }
        public int GetPozitie()
        {
            return pozitie;
        }
       

    }
}
