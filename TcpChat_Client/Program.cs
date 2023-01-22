﻿using System;
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
            SendMessage(socket_sender, name);


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

                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Platypus));

                    MemoryStream stream = new MemoryStream();   // создали КЭШ

                    xmlSerializer.Serialize(stream, platypus);
                    
                    stream.Position = 0;
                    //Platypus platypus2 = xmlSerializer.Deserialize(stream) as Platypus;

                    byte[] bytes = stream.ToArray();

                    // отправляем утконоса
                    //SendMessage(socket, message);
                    socket.Send(bytes);
                }
                if (message == "dumpling")
                {
                    Dumpling dumpling = new Dumpling()
                    {
                        IsFried = true, Name = "Cтрелка",
                        Description = "Очень вкусный, насыщенный, странно себя ведет"
                    };

                    string text = JsonConvert.SerializeObject(dumpling);

                    SendMessage(socket, text);
                }
                else
                {
                    SendMessage(socket, message);
                }

                
            }
        }

        public static void ReceiveMessageForTask(Socket socket)
        {
            while (true)
            {
                string answer = ReceiveMessage(socket);
                Console.WriteLine(answer);
            }
        }

        public static void SendMessage(Socket socket, string message)
        {
            byte[] bytes_answer = Encoding.Unicode.GetBytes(message);
            socket.Send(bytes_answer);
        }

        public static string ReceiveMessage(Socket socket)
        {
            byte[] bytes = new byte[1024];
            int num_bytes = socket.Receive(bytes);
            return Encoding.Unicode.GetString(bytes, 0, num_bytes);
        }
    }
}
