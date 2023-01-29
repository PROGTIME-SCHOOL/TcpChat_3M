using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;                  // для кодирования
using System.Threading.Tasks;
using System.Xml.Serialization;     // xml

using Newtonsoft.Json;              // json

using TcpChat_Library;
using TcpChat_Library.Models;

namespace TcpChat_Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";
            Console.WriteLine("[CLIENT]");

            Socket socket_sender = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            IPAddress address = IPAddress.Parse("127.0.0.1");  // server ip
            IPEndPoint endRemoutePoint = new IPEndPoint(address, 7632);   // server port

            Console.WriteLine("Нажмите Enter для подключения");
            Console.ReadLine();

            // подключаемся к удаленной точке
            socket_sender.Connect(endRemoutePoint);   // lock

            // работа с именем клиента
            Console.Write("Пожалуйста, введите ваше имя: ");
            string name = Console.ReadLine();   
            Utility.SendMessage(socket_sender, name);


            Action<Socket> taskSendMessage = SendMessageForTask;
            IAsyncResult res = taskSendMessage.BeginInvoke(socket_sender, null, null);

            Action<Socket> taskReceiveMessage = ReceiveMessageForTask;
            IAsyncResult resReceive = taskReceiveMessage.BeginInvoke(socket_sender, null, null);


            taskSendMessage.EndInvoke(res);   // lock
            taskReceiveMessage.EndInvoke(resReceive);   // lock

            Console.ReadLine();
        }

        public static void SendMessageForTask(Socket socket)
        {
            while (true)
            {
                string message = Console.ReadLine();   // lock

                if (message == "platypus")
                {
                    Platypus platypus = new Platypus()
                    {
                        Size = 2, Color = "CoolBrown"
                    };

                    Utility.XmlSerializeAndSend(platypus, socket);
                }
                if (message == "dumpling")
                {
                    Dumpling dumpling = new Dumpling()
                    {
                        IsFried = true, Name = "Cтрелка",
                        Description = "Очень вкусный, насыщенный, странно себя ведет"
                    };

                    Utility.JsonSerializeAndSend(dumpling, socket);
                }
                else
                {
                    Utility.SendMessage(socket, message);
                }

                
            }
        }

        public static void ReceiveMessageForTask(Socket socket)
        {
            while (true)
            {
                string answer = Utility.ReceiveMessage(socket);
                Console.WriteLine(answer);
            }
        }
    }
}
