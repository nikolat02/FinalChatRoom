using ConsoleApplication1;
using System;
using System.Net;
using System.Net.Sockets;

namespace ChatRoomServer
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                try
                {
                    User currUser = new User("asdfasfd", "Asdfasdf");
                    IPAddress ipAd = IPAddress.Parse("192.168.101.9"); //192.168.101.9
                                                                       // use local m/c IP address, and 
                                                                       // use the same in the client

                    /* Initializes the Listener */
                    TcpListener myList = new TcpListener(ipAd, 8001);

                    /* Start Listeneting at the specified port */
                    myList.Start();

                    Console.WriteLine("The server is running at port 8001...");
                    Console.WriteLine("The local End point is  :" +
                                      myList.LocalEndpoint);
                    Console.WriteLine("Waiting for a connection.....");

                    Socket s;
                    while (true)
                    {
                        s = myList.AcceptSocket();
                        currUser.startChat(s);
                    }
                    s.Close();
                    myList.Stop();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e); ;
                }
            }

            Console.ReadKey();
        }
    }
}

