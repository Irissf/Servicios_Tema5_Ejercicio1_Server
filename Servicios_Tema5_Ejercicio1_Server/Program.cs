using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/*Realiza un servidor de fecha y hora. Aceptará los comandos: HORA (hora, minutos y segundos)), 
FECHA (día, mes y año), TODO (hora y fecha), APAGAR (El servidor se cierra). Dependiendo del 
comando que reciba enviará la información correspondiente.
Por la brevedad de los mensajes transmitidos, no es necesario que sea multihilo. Y por tanto para 
poder tramitar varios clientes en cuanto se envíe el dato pedido se cerrará la conexión.
Haz un cliente simple en modo gráfico con cuatro botones (uno por comando) y un TextBox o Label 
para mostrar el resultado y probarlo. También tendrá la posibilidad de indicarle la IP y puerto de 
conexión (aunque venga con unos predefinidos) en un formulario de diálogo.*/

namespace Servicios_Tema5_Ejercicio1_Server
{
    class Program
    {
        public static bool flag = true;
        static void Main(string[] args)
        {
            string message = "";

            IPEndPoint ie = new IPEndPoint(IPAddress.Any, 31416);

            //cremaos el socket
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Enlace de socket al puerto
            //Salta excepción si el puerto está ocupado
            try
            {
                socket.Bind(ie);
            }
            catch (SocketException e) when (e.ErrorCode == (int)SocketError.AddressAlreadyInUse)
            {
                //Cuando el error es de puerto en uso
                Console.WriteLine("Puerto ocupado");
            }

            //Esperando una conexión y estableciendo cola de clientes pendientes
            socket.Listen(10);
            //Esperamos y aceptamos la conexion del cliente (socket bloqueante)

            while (flag)
            {
                Console.WriteLine(flag);
                Socket sClient = socket.Accept();

                using (NetworkStream ns = new NetworkStream(sClient))
                using (StreamReader sr = new StreamReader(ns))
                using (StreamWriter sw = new StreamWriter(ns))
                {
                    try
                    {
                        message = sr.ReadLine();
                        if (message != null)
                        {
                            string result = SelectMode(message);
                            sw.WriteLine(result);
                            sw.Flush();
                        }
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
                sClient.Close();
            }
            socket.Close();//Cerrar los socket  
        }

        public static string SelectMode(string message)
        {

            switch (message)
            {
                case "DATE":
                    return DateTime.Now.Date.ToString("dd/MM/yyyy");
                case "TIME":
                    return DateTime.Now.ToString("hh:mm");
                case "DATETIME":
                    return DateTime.Now.ToString();
                case "CLOSE":
                    flag = false;
                    return "CLOSE";
                default:
                    return "COMMAND NOT FOUND";
            }

        }

    }
}


