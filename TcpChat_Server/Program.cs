using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;   // for sockets
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;

using Newtonsoft.Json;

namespace TcpChat_Server
{
    internal class Program
    {
        static string messageFromUser = "";

        static List<User> сlientSockets = new List<User>();

        static void Main(string[] args)
        {
            Console.Title = "Server";
            Console.WriteLine("[SEREVER]");

            // socket TCP
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                ProtocolType.Tcp);

            IPAddress address = IPAddress.Parse("127.0.0.1");

            // создаем endpoint = 127.0.0.1:7632
            IPEndPoint endPoint = new IPEndPoint(address, 7632);

            // привязываем сокет к endpoint
            socket.Bind(endPoint);

            socket.Listen(2);   // переводим сокет в режим СЛУШАТЬ

            Console.WriteLine("Ожидаем звонка от клиента...");

            while (true)
            {
                Socket socket_client = socket.Accept();    // ожидаем звонка от клиента

                User user = new User()
                {
                    Socket = socket_client,
                    Name = "Test"
                };

                сlientSockets.Add(user);

                Console.WriteLine("Клиент на связи!");

                // создаем менеджера - прием сообщений
                Thread threadReceive = new Thread(ReceiveMessageForManager);
                threadReceive.Start(user);

                // создаем менеджера - отсылает сообщения 
                Thread threadSend = new Thread(SendMessageForManager);
                threadSend.Start(user);
            }

            Console.ReadLine();
        }

        public static void SendMessage(Socket socket, string message)
        {
            byte[] bytes_answer = Encoding.Unicode.GetBytes(message);
            socket.Send(bytes_answer);
        }

        public static void SendMessageToAllUsers(string message)
        {
            foreach (var user in сlientSockets)
            {
                SendMessage(user.Socket, message);
            }
        }

        public static void SendMessageForManager(object socketObj)
        {
            User user = (User)socketObj;

            while (true)
            {
                string sendMessage = Console.ReadLine();
                //SendMessage(socket, sendMessage);
                SendMessageToAllUsers(user.Name + ", " + sendMessage);
            }
        }
   
        public static string ReceiveMessage(Socket socket)
        {
            byte[] bytes = new byte[1024];
            int num_bytes = socket.Receive(bytes);
            return Encoding.Unicode.GetString(bytes, 0, num_bytes);
        }

        public static void ReceiveMessageForManager(object socketObj)
        {
            User user = (User)socketObj;

            // получаем данные перед началом работы
            string name = ReceiveMessage(user.Socket);
            user.Name = name;

            // получаем сообзения от клиента
            while (true)
            {
                messageFromUser = ReceiveMessage(user.Socket);

                Console.WriteLine("[" + name + "]: " + messageFromUser);

                // РАСКОДИРОВАТЬ СООБЩЕНИЕ ОТ ПОЛЬЗОВАТЕЛЯ
                // ProcessCommandWord(user.Socket, messageFromUser);
                // ProcessCommandCoding(user.Socket, messageFromUser);

                ProcessCommandJson(user.Socket, messageFromUser);


                #region Receive XML

                /*
                byte[] bytes = new byte[1024];
                int num_bytes = user.Socket.Receive(bytes);

                ProcessCommandXML(user.Socket, bytes, num_bytes);
                */

                #endregion
            }
        }

        private static void ProcessCommandJson(Socket socket, string text)
        {
            Dumpling dumpling = JsonConvert.DeserializeObject<Dumpling>(text);
        }

        private static void ProcessCommandXML(Socket socket, byte[] bytes, int num_bytes)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Platypus));

            MemoryStream stream = new MemoryStream(bytes, 0, num_bytes);

            stream.Position = 0;

            Platypus platypus = xmlSerializer.Deserialize(stream) as Platypus;
        }

        private static void ProcessCommandWord(Socket socket, string command)
        {
            if (messageFromUser == "color")
            {
                Console.WriteLine("Пользователь прислал команду color");

                SendMessage(socket, "Сервер принял вашу команду!");
            }
        }

        private static void ProcessCommandCoding(Socket socket, string text)
        {
            // health, level, money
            // 10,4,5

            int health, level, money;

            string[] numsText = text.Split(',');

            health = int.Parse(numsText[0]);
            level = int.Parse(numsText[1]);
            money = int.Parse(numsText[2]);

            Console.WriteLine($"Health: {health}, Level: {level}, Money: {money}");
        }
    }

    public class User
    {
        public string Name { get; set; }
        public Socket Socket { get; set; }
    }
}
