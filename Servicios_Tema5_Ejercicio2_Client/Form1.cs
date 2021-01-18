using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Servicios_Tema5_Ejercicio2_Client
{
    public partial class Form1 : Form
    {
        Button btn;
        const string IP_SERVER = "127.0.0.1";
        int port;

        public Form1()
        {
            InitializeComponent();
            port = 31416;


        }

        private void ClickOn(object sender, EventArgs e)
        {
            btn = (Button)sender;

            if (btn.Name == "button1")
            {
                btn.Tag = "DATE";
            }
            else
            if (btn.Name == "button2")
            {
                btn.Tag = "TIME";
            }
            else
            if (btn.Name == "button3")
            {
                btn.Tag = "DATETIME";
            }
            else
            if (btn.Name == "button4")
            {
                btn.Tag = "CLOSE";
            }

            //Indicamos el servidor al que nos queremos conectar y su puerto
            IPEndPoint ipEP = new IPEndPoint(IPAddress.Parse(IP_SERVER), port);

            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //El cliente lanza una petición para conectarse al servidor mediante la petición connect
                server.Connect(ipEP);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return;//si el puerto está ocupado o algún error me saca del programa en el main o de una función
            }

            using (NetworkStream ns = new NetworkStream(server))
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {

                if (btn.Tag != null)
                {

                    sw.WriteLine(btn.Tag);
                    sw.Flush();
                    label1.Text = sr.ReadLine();
                    if (btn.Tag == "CLOSE")
                    {
                        server.Close();
                        this.Close();
                    }
                }
            }
            server.Close();
        }
    }
}
