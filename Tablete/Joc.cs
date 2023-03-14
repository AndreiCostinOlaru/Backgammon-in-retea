using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tablete
{
    public class Joc
    {
        private Zar z1, z2, z3, z4;
        private int roll1, roll2;
        private Button butonRoll = new Button();
        private Button butonSkip = new Button();
        private bool skip;
        private Label labelRunda = new Label();
        private fJoc f;
        private Piesa[] piese = new Piesa[30];
        private Button butonMuta = new Button();
        private bool rundaAlb;
        private Button butonExit = new Button();
        private StreamWriter scriere;
        private int pieseAlbeInlaturate;
        private int pieseNegreInlaturate;
        private int pieseAlbeScoase;
        private int pieseNegreScoase;
        private int idClick;
        private int valoareZarClick;
        public Joc(fJoc f)
        {
            this.f = f;
        }
        public void Initializare()
        {
            rundaAlb = true;
            butonRoll.Name = "roll";
            butonRoll.Visible = false;
            f.GetTableLayoutPanel().Controls.Add(butonRoll, 14, 7);
            f.GetTableLayoutPanel().Controls.Add(butonMuta, 14, 8);
            f.GetTableLayoutPanel().Controls.Add(butonSkip, 14, 13);
            f.GetTableLayoutPanel().Controls.Add(butonExit, 14, 14);
            butonRoll.Click += new EventHandler(butonRoll_Click);
            butonRoll.Text = "Roll";
            butonRoll.Dock = System.Windows.Forms.DockStyle.Fill;
            butonMuta.Click += new EventHandler(butonMuta_Click);
            butonMuta.Text = "Muta";
            butonMuta.Visible = false;
            butonMuta.Dock = System.Windows.Forms.DockStyle.Fill;
            butonRoll.Click += new EventHandler(butonRoll_Click);
            butonSkip.Click += new EventHandler(butonSkip_Click);
            butonSkip.Text = "Skip";
            butonSkip.Dock = System.Windows.Forms.DockStyle.Fill;
            butonSkip.Visible = false;
            butonExit.Click += new EventHandler(butonExit_Click);
            butonExit.Text = "Exit";
            butonExit.Dock = System.Windows.Forms.DockStyle.Fill;
            z1 = new Zar(f, 1);
            z2 = new Zar(f, 2);
            z3 = new Zar(f, 3);
            z4 = new Zar(f, 4);
            z1.SetCell(0, 8);
            z2.SetCell(0, 7);
            z3.SetCell(1, 8);
            z4.SetCell(1, 7);
            labelRunda.AutoSize = true;
            labelRunda.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            labelRunda.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            labelRunda.BackColor = System.Drawing.Color.Transparent;
            labelRunda.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            f.GetTableLayoutPanel().Controls.Add(labelRunda, 14, 0);
            for (int i = 0; i < 30; i++)
            {
                if (i < 2) piese[i] = new PiesaAlba(f, i, 1);
                else if (i >= 2 && i <= 6) piese[i] = new PiesaAlba(f, i, 12);
                else if (i > 6 && i <= 9) piese[i] = new PiesaAlba(f, i, 17);
                else if (i > 9 && i < 15) piese[i] = new PiesaAlba(f, i, 19);
                else if (i >= 15 && i <= 19) piese[i] = new PiesaNeagra(f, i, 6);
                else if (i > 19 && i <= 22) piese[i] = new PiesaNeagra(f, i, 8);
                else if (i > 22 && i <= 27) piese[i] = new PiesaNeagra(f, i, 13);
                else piese[i] = new PiesaNeagra(f, i, 24);
            }
            pieseAlbeInlaturate = 0;
            pieseNegreInlaturate = 0;
            pieseAlbeScoase = 0;
            pieseNegreScoase = 0;
        }
        public void Jocul()
        {
            scriere = new StreamWriter(f.GetNetworkStream());
            scriere.AutoFlush = true;
            if (rundaAlb == true)
            {
                foreach (Control ctr in f.GetTableLayoutPanel().Controls)
                {
                    if (ctr.Name == "roll")
                    {
                        ctr.Enabled = true;
                    }
                    else
                    {
                        ctr.Enabled = false;
                    }
                }
            }
            else
            {

                foreach (Control ctr in f.GetTableLayoutPanel().Controls)
                {
                    if (ctr.Name == "roll")
                    {
                        ctr.Enabled = false;
                    }
                    else
                    {
                        ctr.Enabled = true;
                    }
                }
            }
        }
        public void Asculta(string[] aux)
        {
            if (aux[0] == "exit")
            {
                f.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show("Adversarul a parasit jocul!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    f.SetFalseAscult();
                    Environment.Exit(0);
                });
            }
            if (aux[0] == "skip")
            {
                skip = true;
            }
            if (aux[0] == "zar")
            {
                Console.WriteLine("zar in asculta:" + aux[1]);
                valoareZarClick = Int32.Parse(aux[2]);
                f.Invoke((MethodInvoker)delegate
                {
                    switch (Int32.Parse(aux[1]))
                    {
                        case 1: z1.SetInvisibileButonZar(); break;
                        case 2: z2.SetInvisibileButonZar(); break;
                        case 3: z3.SetInvisibileButonZar(); break;
                        case 4: z4.SetInvisibileButonZar(); break;
                    }
                });
            }
            if (aux[0] == "castiga")
            {
                f.Invoke((MethodInvoker)delegate
                {
                    if (aux[1] == "alb")
                    {
                        MessageBox.Show("Alb castiga!", "", MessageBoxButtons.OK, MessageBoxIcon.None);
                        butonMuta.Visible = false;
                        butonRoll.Visible = false;
                        butonSkip.Visible = false;
                        z1.SetInvisibileButonZar();
                        z2.SetInvisibileButonZar();
                        z3.SetInvisibileButonZar();
                        z4.SetInvisibileButonZar();
                        labelRunda.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
                        labelRunda.Text = "Alb a castigat!";
                    }
                    else
                    {
                        MessageBox.Show("Negru castiga!", "", MessageBoxButtons.OK, MessageBoxIcon.None);
                        butonMuta.Visible = false;
                        butonRoll.Visible = false;
                        butonSkip.Visible = false;
                        z1.SetInvisibileButonZar();
                        z2.SetInvisibileButonZar();
                        z3.SetInvisibileButonZar();
                        z4.SetInvisibileButonZar();
                        labelRunda.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F);
                        labelRunda.Text = "Negru a castigat!";
                    }
                });
            }
            if (aux[0] == "roll")
            {
                f.Invoke((MethodInvoker)delegate
                {
                    butonMuta.Visible = true;
                    butonSkip.Visible = true;
                    if ((!z1.GetVisibileButonZar() && !z2.GetVisibileButonZar() && !z3.GetVisibileButonZar() && !z4.GetVisibileButonZar() && valoareZarClick == 0) || skip)
                    {
                        valoareZarClick = 0;
                        roll1 = z1.Roll(Int32.Parse(aux[1]), true);
                        roll2 = z2.Roll(Int32.Parse(aux[2]), true);
                        z3.Roll(roll1, true);
                        z4.Roll(roll1, true);
                        if (roll1 == roll2)
                        {
                            z1.SetVisibileButonZar();
                            z2.SetVisibileButonZar();
                            z3.SetVisibileButonZar();
                            z4.SetVisibileButonZar();
                        }
                        else
                        {
                            z1.SetVisibileButonZar();
                            z2.SetVisibileButonZar();
                            z3.SetInvisibileButonZar();
                            z4.SetInvisibileButonZar();
                        }
                        rundaAlb = !rundaAlb;
                        if (rundaAlb) labelRunda.Text = "Alb muta!";
                        else labelRunda.Text = "Negru muta!";
                        Jocul();
                    }
                    skip = false;
                });
            }
            if (aux[0] == "muta")
            {
                f.Invoke((MethodInvoker)delegate
                {
                    piese[Int32.Parse(aux[1])].Muta(Int32.Parse(aux[2]));
                });
                valoareZarClick = 0;
            }
        }
        public void VisibleButonRoll()
        {
            butonRoll.Visible = true;
        }
        private void butonExit_Click(object sender, EventArgs e)
        {
            if (scriere != null)
            {
                scriere = new StreamWriter(f.GetNetworkStream());
                scriere.AutoFlush = true;
                scriere.WriteLine("exit");
            }
            Environment.Exit(0);
        }
        private void butonSkip_Click(object sender, EventArgs e)
        {
            scriere = new StreamWriter(f.GetNetworkStream());
            scriere.AutoFlush = true;
            Jocul();
            scriere.WriteLine("skip");
            skip = true;
            butonRoll_Click(sender, e);
        }
        private void butonMuta_Click(object sender, EventArgs e)
        {
            scriere = new StreamWriter(f.GetNetworkStream());
            scriere.AutoFlush = true;
            int i = idClick;
            int k;
            if (rundaAlb && idClick >= 15)
            {
                MessageBox.Show("Alege o piesa alba!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!rundaAlb && idClick < 15)
            {
                MessageBox.Show("Alege o piesa neagra!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (piese[i].VerificaDacaInlaturat())
            {
                if (!piese[i].Muta(valoareZarClick)) MessageBox.Show("Mutare invalida", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    scriere.WriteLine("muta*" + i + "*" + valoareZarClick);
                    valoareZarClick = 0;
                }
            }
            else
            {
                if (piese[i].IsTop() && piese[i].Muta(valoareZarClick))
                {
                    scriere.WriteLine("muta*" + i + "*" + valoareZarClick);
                    valoareZarClick = 0;
                }

                else
                {
                    int j = piese[i].GetPozitie();
                    for (k = 0; k < 30; k++)
                    {
                        if (piese[k].GetPozitie() == j && piese[k].IsTop())
                            break;
                    }
                    if (k != 30 && piese[k].Muta(valoareZarClick))
                    {
                        scriere.WriteLine("muta*" + k + "*" + valoareZarClick);
                        valoareZarClick = 0;
                    }
                    else if (k != 30) MessageBox.Show("Mutare invalida", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (pieseAlbeScoase == 15)
            {
                MessageBox.Show("Alb castiga!", "", MessageBoxButtons.OK, MessageBoxIcon.None);
                butonMuta.Visible = false;
                butonRoll.Visible = false;
                butonSkip.Visible = false;
                z1.SetInvisibileButonZar();
                z2.SetInvisibileButonZar();
                z3.SetInvisibileButonZar();
                z4.SetInvisibileButonZar();
                labelRunda.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F);
                labelRunda.Text = "Alb a castigat!";
                scriere.WriteLine("castiga*alb");
            }
            if (pieseNegreScoase == 15)
            {
                MessageBox.Show("Negru castiga!", "", MessageBoxButtons.OK, MessageBoxIcon.None);
                butonMuta.Visible = false;
                butonRoll.Visible = false;
                butonSkip.Visible = false;
                z1.SetInvisibileButonZar();
                z2.SetInvisibileButonZar();
                z3.SetInvisibileButonZar();
                z4.SetInvisibileButonZar();
                labelRunda.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F);
                labelRunda.Text = "Negru a castigat!";
                scriere.WriteLine("castiga*negru");
            }
        }
        private void butonRoll_Click(object sender, EventArgs e)
        {
            scriere = new StreamWriter(f.GetNetworkStream());
            scriere.AutoFlush = true;
            butonMuta.Visible = true;
            butonSkip.Visible = true;
            if ((!z1.GetVisibileButonZar() && !z2.GetVisibileButonZar() && !z3.GetVisibileButonZar() && !z4.GetVisibileButonZar() && valoareZarClick == 0) || skip)
            {
                valoareZarClick = 0;
                roll1 = z1.Roll();
                Thread.Sleep(1);
                roll2 = z2.Roll();
                z3.Roll(roll1, true);
                z4.Roll(roll1, true);
                if (roll1 == roll2)
                {
                    scriere.WriteLine("roll*" + roll1 + "*" + roll1 + "*" + roll1 + "*" + roll1);
                    z1.SetVisibileButonZar();
                    z2.SetVisibileButonZar();
                    z3.SetVisibileButonZar();
                    z4.SetVisibileButonZar();
                }
                else
                {
                    scriere.WriteLine("roll*" + roll1 + "*" + roll2);
                    z1.SetVisibileButonZar();
                    z2.SetVisibileButonZar();
                    z3.SetInvisibileButonZar();
                    z4.SetInvisibileButonZar();
                }
                rundaAlb = !rundaAlb;
                if (rundaAlb) labelRunda.Text = "Alb muta!";
                else labelRunda.Text = "Negru muta!";
                Jocul();
            }
            skip = false;
        }
        
        public int GetPieseAlbeInlaturate()
        {
            return pieseAlbeInlaturate;
        }
        public void SetPieseAlbeInlaturate(int valoare)
        {
            pieseAlbeInlaturate += valoare;
        }
        public int GetPieseNegreInlaturate()
        {
            return pieseNegreInlaturate;
        }
        public void SetPieseNegreInlaturate(int valoare)
        {
            pieseNegreInlaturate += valoare;
        }
        public int GetPieseAlbeScoase()
        {
            return pieseAlbeScoase;
        }
        public void SetPieseAlbeScoase(int valoare)
        {
            pieseAlbeScoase += valoare;
        }
        public int GetPieseNegreScoase()
        {
            return pieseNegreScoase;
        }
        public void SetPieseNegreScoase(int valoare)
        {
            pieseNegreScoase += valoare;
        }
        public void SetIdClick(int id)
        {
            idClick = id;
        }
        public int GetValoareZarClick()
        {
            return valoareZarClick;
        }
        public void SetValoareZarClick(int valoare)
        {
            valoareZarClick = valoare;
        }
    }
}


