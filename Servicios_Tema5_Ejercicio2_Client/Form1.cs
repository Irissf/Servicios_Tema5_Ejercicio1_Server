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
        IPAddress ip;
        int port;
        Form2 form2;
        bool correctIP = false;

        public Form1()
        {
            form2 = new Form2();
           

            while (!correctIP)
            {
                DialogResult result;
                result = form2.ShowDialog();
                switch (result)
                {
                    case DialogResult.OK:
                        if (form2.textBox1.Text.Length > 0)
                        {
                            try
                            {
                                ip = IPAddress.Parse(form2.textBox1.Text);
                                //acepta 122, ya que rellena y el 122 lo pone como 0.0.0.122, por eso entra 
                                correctIP = true;
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("La ip es incorrecta");
                            }
                        }

                        break;
                    default:
                        break;
                }

            }
            

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
            IPEndPoint ipEP = new IPEndPoint(ip, port);

            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //El cliente lanza una petición para conectarse al servidor mediante la petición connect
                server.Connect(ipEP);
            }
            catch (SocketException ex)
            {
                MessageBox.Show("No se ha podido establecer la conexión con el servidor");
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
                    if ((string)btn.Tag == "CLOSE")
                    {
                        server.Close();
                        this.Close();
                    }
                }
            }
            server.Close();
        }

        private void ChangeValue(object sender, EventArgs e)
        {

        }
    }
}
