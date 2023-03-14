using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tablete
{
    public partial class fJoc : Form
    {
        private TcpClient client;
        private Joc table;
        private NetworkStream clientStream;
        private bool ascult;
        private Thread t;
        public fJoc()
        {
            table = new Joc(this);
            InitializeComponent();
            table.Initializare();

        }
        private void AscultaClient()
        {
            StreamReader citire = new StreamReader(clientStream);
            String dateClient;
            while (ascult)
            {
                dateClient = citire.ReadLine();
                string[] aux = dateClient.Split('*');
                Console.WriteLine(dateClient);
                table.Asculta(aux);
                
            }
        }
        public TableLayoutPanel GetTableLayoutPanel()
        {
            return tableLayoutPanel;
        }
        private void butonConnect_Click(object sender, EventArgs e)
        {
            if (butonConnect.Text == "Connect")
            {
                if (textBoxAdresa.Text.Length > 0)
                {
                    client = new TcpClient(textBoxAdresa.Text, 3000);
                    ascult = true;
                    t = new Thread(new ThreadStart(AscultaClient));
                    t.Start();
                    clientStream = client.GetStream();



                    labelAdresa.Visible = false;
                    textBoxAdresa.Visible = false;
                    butonConnect.Text = "Disconnect";
                    table.VisibleButonRoll();
                    TableLayoutPanelCellPosition cell = new TableLayoutPanelCellPosition(14, 13);
                    tableLayoutPanel.SetCellPosition(butonConnect, cell);
                }
                else
                {
                    MessageBox.Show("Specificati adresa de IP");
                }
            }
            else
            {
                ascult = false;
                t.Abort();
                StreamWriter scriere = new StreamWriter(clientStream);
                scriere.AutoFlush = true; 
                scriere.WriteLine("exit");
                Environment.Exit(0);
            }

        }
        public NetworkStream GetNetworkStream()
        {
            return clientStream;
        }
        public Joc GetJoc()
        {
            return table;
        }
        public void SetFalseAscult()
        {
            ascult = false;
        }
        private void fJoc_FormClosed(object sender, FormClosedEventArgs e)
        {
            ascult = false;
            if(t!=null) t.Abort();
            if (clientStream != null)
            {
                StreamWriter scriere = new StreamWriter(clientStream);
                scriere.AutoFlush = true;
                scriere.WriteLine("exit");
                client.Close();
            }
            Environment.Exit(0);
        }
    }
}

