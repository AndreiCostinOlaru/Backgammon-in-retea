using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tablete
{
    class PiesaNeagra : Piesa
    {
        public PiesaNeagra(fJoc f, int id, int pozitie)
        {
            this.f = f;
            this.id = id;
            this.pozitie = pozitie;
            piesaImg.Image = Properties.Resources.brown;
            if (pozitie <= 6) f.GetTableLayoutPanel().Controls.Add(piesaImg, 14 - pozitie, 14 - cateNegre[pozitie]);
            if (pozitie > 6 && pozitie <= 12) f.GetTableLayoutPanel().Controls.Add(piesaImg, 13 - pozitie, 14 - cateNegre[pozitie]);
            if (pozitie > 12 && pozitie <= 18) f.GetTableLayoutPanel().Controls.Add(piesaImg, pozitie - 12, cateNegre[pozitie] + 1);
            if (pozitie > 18) f.GetTableLayoutPanel().Controls.Add(piesaImg, pozitie - 11, cateNegre[pozitie] + 1);
            cateNegre[pozitie]++;
        }

        public override bool ToateInCasa()
        {
            int suma = 0;
            for (int i = 1; i <= 6; i++) suma += cateNegre[i];
            if (suma == 15 - f.GetJoc().GetPieseNegreScoase()) return true;
            return false;
        }
        public override bool Muta(int valoareZar)
        {
            if (f.GetJoc().GetPieseNegreInlaturate() == 0)
            {
                TableLayoutPanelCellPosition cellPosition = f.GetTableLayoutPanel().GetCellPosition(piesaImg);
                if (pozitie - valoareZar >= 1 && cateAlbe[pozitie - valoareZar] > 1) return false;
                if (pozitie - valoareZar < 1 && !ToateInCasa())
                {
                    Console.WriteLine("NU!");
                    return false;
                }
                if (ToateInCasa() && pozitie - valoareZar < 0 && !VerificaPozitiiAnterioare())
                {
                    return false;
                }
                if (pozitie - valoareZar == 0 && ToateInCasa())
                {
                    piesaImg.Visible = false;
                    cellPosition.Column = 0;
                    cellPosition.Row = 0;
                    cateNegre[pozitie]--;
                    pozitie = 0;
                    f.GetJoc().SetPieseNegreScoase(1);
                }
                if (ToateInCasa() && pozitie - valoareZar < 0 && VerificaPozitiiAnterioare())
                {
                    piesaImg.Visible = false;
                    cellPosition.Column = 0;
                    cellPosition.Row = 0;
                    cateNegre[pozitie]--;
                    pozitie = 0;
                    f.GetJoc().SetPieseNegreScoase(1);
                }
                if (pozitie - valoareZar < 7 && pozitie - valoareZar >= 1 && cateAlbe[pozitie - valoareZar] <= 1)
                {
                    cateNegre[pozitie]--;
                    cellPosition.Column += valoareZar;
                    if (cellPosition.Column >= 7 && pozitie > 6)
                    {
                        Console.WriteLine("intra in if cu pozitia: " + pozitie);
                        cellPosition.Column++;
                    }
                    pozitie -= valoareZar;
                    cellPosition.Row = 14 - cateNegre[pozitie];
                    cateNegre[pozitie]++;
                    Console.WriteLine("coloana:");
                    Console.WriteLine(cellPosition.Column);
                    Console.WriteLine("cate negre:");
                    Console.WriteLine(cateNegre[pozitie]);
                    Console.WriteLine("pozitie:");
                    Console.WriteLine(pozitie);
                }
                if (pozitie - valoareZar > 6 && pozitie - valoareZar <= 12 && cateAlbe[pozitie - valoareZar] <= 1)
                {
                    cateNegre[pozitie]--;
                    pozitie -= valoareZar;
                    cellPosition.Column = 13 - pozitie;
                    cellPosition.Row = 14 - cateNegre[pozitie];
                    cateNegre[pozitie]++;
                    Console.WriteLine(pozitie);
                }
                if (pozitie - valoareZar > 12 && cateAlbe[pozitie - valoareZar] <= 1)
                {
                    Console.WriteLine("caz1");
                    cellPosition.Column -= valoareZar;
                    cateNegre[pozitie]--;
                    if (cellPosition.Column < 8 && pozitie > 18)
                    {
                        Console.Write("Pozitie in if: ");
                        Console.WriteLine(pozitie);
                        cellPosition.Column--;
                        Console.WriteLine("Intra in if");
                    }
                    pozitie -= valoareZar;
                    Console.WriteLine(cellPosition.Column);
                    cellPosition.Row = cateNegre[pozitie] + 1;
                    cateNegre[pozitie]++;
                    Console.WriteLine(pozitie);
                    Console.WriteLine(cellPosition.Column);
                }
                if (cateAlbe[pozitie] == 1)
                {
                    TableLayoutPanelCellPosition centerCellPosition = new TableLayoutPanelCellPosition(7, 7);
                    f.GetTableLayoutPanel().SetCellPosition(f.GetTableLayoutPanel().GetControlFromPosition(cellPosition.Column, cellPosition.Row), centerCellPosition);
                    f.GetJoc().SetPieseAlbeInlaturate(1);
                    cateAlbe[pozitie] = 0;
                }
                f.GetTableLayoutPanel().SetCellPosition(piesaImg, cellPosition);
                return true;
            }
            else if (valoareZar != 0 && VerificaDacaInlaturat() && cateAlbe[25 - valoareZar] <= 1)
            {
                pozitie = 25 - valoareZar;
                TableLayoutPanelCellPosition newCellPosition = new TableLayoutPanelCellPosition(pozitie - 11, cateNegre[25 - valoareZar] + 1);
                if (cateAlbe[pozitie] == 1)
                {
                    TableLayoutPanelCellPosition centerCellPosition = new TableLayoutPanelCellPosition(7, 7);
                    f.GetTableLayoutPanel().SetCellPosition(f.GetTableLayoutPanel().GetControlFromPosition(newCellPosition.Column, newCellPosition.Row), centerCellPosition);
                    f.GetJoc().SetPieseAlbeInlaturate(1);
                    cateAlbe[pozitie] = 0;
                }
                f.GetTableLayoutPanel().SetCellPosition(piesaImg, newCellPosition);
                f.GetJoc().SetPieseNegreInlaturate(-1);
                cateNegre[25 - valoareZar]++;
                return true;
            }
            return false;
        }
        public override bool IsTop()
        {
            if (pozitie <= 12) return f.GetTableLayoutPanel().GetCellPosition(piesaImg).Row == (15 - cateNegre[pozitie]);
            else return f.GetTableLayoutPanel().GetCellPosition(piesaImg).Row == cateNegre[pozitie];
        }
        private bool VerificaPozitiiAnterioare()
        {
            int poz = 6 - pozitie;
            for (int i = 1; i <= poz; i++)
            {
                if (cateNegre[pozitie + i] != 0) return false;
            }
            return true;
        }
    }
}