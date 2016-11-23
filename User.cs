using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class User
    {
        private static List<Socket> clientLIst = new List<Socket>();
        private Socket clientSocket;
        private string ipAddress;
        private string userName;
        public string UserName
        {
            get { return userName; }
            private set
            {
                userName = value;
            }
        }

        public string _IPAddress
        {
            get { return ipAddress; }
            private set
            {
                ipAddress = value;
            }
        }


        public User(string user, string ip)
        {
            UserName = user;
            _IPAddress = ip;
        }

        private void handleChatThread()
        {
            try
            {


                //Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);
                bool firstRequest = true;
                string username = "";
                byte[] b = new byte[100];
                while (true)
                {
                    int k = clientSocket.Receive(b);
                    Console.WriteLine("Recieved...");
                    for (int i = 0; i < k; i++)
                        Console.Write(Convert.ToChar(b[i]));

                    if (firstRequest)
                    {
                        username = System.Text.Encoding.Default.GetString(b);
                        firstRequest = false;
                    }
                    else
                    {
                        ASCIIEncoding asen = new ASCIIEncoding();       //SOLID: Dependency Inversion Principle
                        byte[] message = asen.GetBytes(username + ":  " + System.Text.Encoding.Default.GetString(b));

                        foreach (Socket s in clientLIst)
                        {
                            try
                            {
                                string test = System.Text.Encoding.Default.GetString(message);
                                s.Send(message);
                            }
                            catch (SocketException ex)
                            {
                                clientLIst.Remove(s);
                            }
                        }
                    }

                    /* clean up */
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }

        public void startChat(Socket s)     //SOLID: Good use of Simple Design
        {
            clientSocket = s;
            clientLIst.Add(s);

            Thread newThread = new Thread(handleChatThread);
            newThread.Start();

        }
    }

}
