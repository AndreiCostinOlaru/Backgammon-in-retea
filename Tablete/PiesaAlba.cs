using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tablete
{
    class PiesaAlba : Piesa
    {
        public PiesaAlba(fJoc f, int id, int pozitie)
        {
            this.f = f;
            this.id = id;
            this.pozitie = pozitie;
            piesaImg.Image = Properties.Resources.white;
            if (pozitie <= 6) f.GetTableLayoutPanel().Controls.Add(piesaImg, 14 - pozitie, 14 - cateAlbe[pozitie]);
            if (pozitie > 6 && pozitie <= 12) f.GetTableLayoutPanel().Controls.Add(piesaImg, 13 - pozitie, 14 - cateAlbe[pozitie]);
            if (pozitie > 12 && pozitie <= 18) f.GetTableLayoutPanel().Controls.Add(piesaImg, pozitie - 12, cateAlbe[pozitie] + 1);
            if (pozitie > 18) f.GetTableLayoutPanel().Controls.Add(piesaImg, pozitie - 11, cateAlbe[pozitie] + 1);
            cateAlbe[pozitie]++;
        }

        public override bool IsTop()
        {
            if (pozitie <= 12) return f.GetTableLayoutPanel().GetCellPosition(piesaImg).Row == (15 - cateAlbe[pozitie]);
            else if (pozitie != 25) return f.GetTableLayoutPanel().GetCellPosition(piesaImg).Row == cateAlbe[pozitie];
            else return false;
        }

        public override bool ToateInCasa()
        {
            int suma = 0;
            for (int i = 19; i < 25; i++) suma += cateAlbe[i];
            if (suma == 15 - f.GetJoc().GetPieseAlbeScoase()) return true;
            return false;
        }

        public override bool Muta(int valoareZar)
        {
            if (f.GetJoc().GetPieseAlbeInlaturate() == 0)
            {
                TableLayoutPanelCellPosition cellPosition = f.GetTableLayoutPanel().GetCellPosition(piesaImg);
                if (pozitie + valoareZar <= 24 && cateNegre[pozitie + valoareZar] > 1) return false;
                if (pozitie + valoareZar > 24 && !ToateInCasa())
                {
                    Console.WriteLine("NU!");
                    return false;
                }
                if (ToateInCasa() && pozitie + valoareZar > 25 && !VerificaPozitiiAnterioare())
                {
                    return false;
                }
                if (ToateInCasa() && pozitie + valoareZar == 25)
                {
                    piesaImg.Visible = false;
                    cateAlbe[pozitie]--;
                    cellPosition.Column = 14;
                    cellPosition.Row = 14;
                    pozitie = 25;
                    f.GetJoc().SetPieseAlbeScoase(1);

                }
                if (ToateInCasa() && pozitie + valoareZar > 25 && VerificaPozitiiAnterioare() && pozitie != 25)
                {
                    piesaImg.Visible = false;
                    cateAlbe[pozitie]--;
                    cellPosition.Column = 14;
                    cellPosition.Row = 14;
                    pozitie = 25;
                    f.GetJoc().SetPieseAlbeScoase(1);

                }
                if (pozitie + valoareZar > 18 && pozitie + valoareZar <= 24 && cateNegre[pozitie + valoareZar] <= 1)
                {
                    cateAlbe[pozitie]--;
                    cellPosition.Column += valoareZar;
                    if (cellPosition.Column >= 7 && pozitie <= 18)
                    {
                        cellPosition.Column++;
                    }
                    pozitie += valoareZar;
                    cellPosition.Row = cateAlbe[pozitie] + 1;
                    cateAlbe[pozitie]++;
                    Console.WriteLine("coloana:");
                    Console.WriteLine(cellPosition.Column);
                    Console.WriteLine("cate albe:");
                    Console.WriteLine(cateAlbe[pozitie]);
                    Console.WriteLine("pozitie:");
                    Console.WriteLine(pozitie);
                }
                if (pozitie + valoareZar > 12 && pozitie + valoareZar <= 18 && cateNegre[pozitie + valoareZar] <= 1)
                {
                    cateAlbe[pozitie]--;
                    pozitie += valoareZar;
                    cellPosition.Column = pozitie - 12;
                    cellPosition.Row = cateAlbe[pozitie] + 1;
                    cateAlbe[pozitie]++;
                    Console.WriteLine(pozitie);
                }
                if (pozitie + valoareZar <= 12 && cateNegre[pozitie + valoareZar] <= 1)
                {
                    Console.WriteLine("caz1");
                    cellPosition.Column -= valoareZar;
                    cateAlbe[pozitie]--;
                    if (cellPosition.Column <= 7 && pozitie < 7)
                    {
                        cellPosition.Column--;
                    }
                    pozitie += valoareZar;
                    Console.WriteLine(cellPosition.Column);
                    cellPosition.Row = 14 - cateAlbe[pozitie];
                    cateAlbe[pozitie]++;
                    Console.WriteLine(pozitie);
                    Console.WriteLine(cellPosition.Column);
                }
                if (pozitie < 25 && cateNegre[pozitie] == 1)
                {
                    Control auxControl = f.GetTableLayoutPanel().GetControlFromPosition(cellPosition.Column, cellPosition.Row);
                    TableLayoutPanelCellPosition centerCellPosition = new TableLayoutPanelCellPosition(7, 7);
                    f.GetTableLayoutPanel().SetCellPosition(auxControl, centerCellPosition);
                    f.GetJoc().SetPieseNegreInlaturate(1);
                    cateNegre[pozitie] = 0;
                }
                f.GetTableLayoutPanel().SetCellPosition(piesaImg, cellPosition);
                return true;
            }
            else if (valoareZar != 0 && VerificaDacaInlaturat() && cateNegre[valoareZar] <= 1)
            {
                pozitie = valoareZar;
                TableLayoutPanelCellPosition newCellPosition = new TableLayoutPanelCellPosition(14 - pozitie, 14 - cateAlbe[valoareZar]);
                if (cateNegre[pozitie] == 1)
                {
                    TableLayoutPanelCellPosition centerCellPosition = new TableLayoutPanelCellPosition(7, 7);
                    f.GetTableLayoutPanel().SetCellPosition(f.GetTableLayoutPanel().GetControlFromPosition(newCellPosition.Column, newCellPosition.Row), centerCellPosition);
                    f.GetJoc().SetPieseNegreInlaturate(1);
                    cateNegre[pozitie] = 0;
                }
                f.GetTableLayoutPanel().SetCellPosition(piesaImg, newCellPosition);
                f.GetJoc().SetPieseAlbeInlaturate(-1);
                cateAlbe[valoareZar]++;
                return true;
            }
            return false;
        }

        private bool VerificaPozitiiAnterioare()
        {
            int poz = pozitie - 18;
            for (int i = 1; i <= poz; i++)
            {
                if (cateAlbe[pozitie - i] != 0) return false;
            }
            return true;
        }
    }
}

