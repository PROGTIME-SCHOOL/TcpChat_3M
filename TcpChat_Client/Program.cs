using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;                  // для кодирования
using System.Threading.Tasks;

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

            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint endRemoutePoint = new IPEndPoint(address, 7632);

            Console.WriteLine("Нажмите Enter для подключения");
            Console.ReadLine();

            // подключаемся к удаленной точке
            socket_sender.Connect(endRemoutePoint);

            while (true)
            {
                // отправка сообщения на сервер
                Console.WriteLine("Введите сообщение для отправки на сервер:");
                string message = Console.ReadLine();
                byte[] bytes = Encoding.Unicode.GetBytes(message);
                socket_sender.Send(bytes);
                Console.WriteLine("Посылка \"" + message + "\" отправлена на сервер!");

                // получаем ответ от сервера
                byte[] byte_answers = new byte[1024];
                int num_bytes = socket_sender.Receive(byte_answers);
                string answer = Encoding.Unicode.GetString(byte_answers, 0, num_bytes);
                Console.WriteLine(answer);
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
