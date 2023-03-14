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
        private TcpListener server;
        private String dateServer;
        private Thread t;
        private Joc table;
        private bool workThread;
        private NetworkStream streamServer;
        public fJoc()
        {
            server = new TcpListener(System.Net.IPAddress.Any, 3000);
            server.Start();
            t = new Thread(new ThreadStart(AscultaServer));
            workThread = true;
            t.Start();
            table = new Joc(this);
            InitializeComponent();
            table.Initializare();
        }

        public void AscultaServer()
        {
            while (workThread)
            {
                Socket socketServer = server.AcceptSocket();
                streamServer = new NetworkStream(socketServer);
                StreamReader citireServer = new StreamReader(streamServer);
                table.VisibleButonRoll();

                while (workThread)
                {
                    dateServer = citireServer.ReadLine();
                    if (dateServer == null) break;
                    if (dateServer == "exit") workThread = false;
                    string[] aux = dateServer.Split('*');
                    Console.Write(dateServer);
                    table.Asculta(aux);

                }
                streamServer.Close();
                socketServer.Close();
            }
        }
        public TableLayoutPanel GetTableLayoutPanel()
        {
            return tableLayoutPanel;
        }
        public NetworkStream GetNetworkStream()
        {
            return streamServer;
        }
        public Joc GetJoc()
        {
            return table;
        }
        public void SetFalseWorkThread()
        {
            workThread = false;
        }
        private void fJoc_FormClosed(object sender, FormClosedEventArgs e)
        {
            workThread = false;
            if (streamServer != null)
            {
                StreamWriter scriere = new StreamWriter(streamServer);
                scriere.AutoFlush = true;
                scriere.WriteLine("exit");
                streamServer.Close();
            }
            Environment.Exit(0);
        }
    }
}
