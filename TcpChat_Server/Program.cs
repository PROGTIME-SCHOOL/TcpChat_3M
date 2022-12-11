using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;   // for sockets
using System.Text;
using System.Threading.Tasks;

namespace TcpChat_Server
{
    internal class Program
    {
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

            socket.Listen(1);   // переводим сокет в режим СЛУШАТЬ

            Console.WriteLine("Ожидаем звонка от клиента...");

            Socket socket_client = socket.Accept();    // ожидаем звонка от клиента

            Console.WriteLine("Клиент на связи!");

            while (true)
            {
                // получение сообщения от клиента
                byte[] bytes = new byte[1024];
                int num_bytes = socket_client.Receive(bytes);
                string textFromClient = Encoding.Unicode.GetString(bytes, 0, num_bytes);
                Console.WriteLine(textFromClient);

                // ответное сообщение от сервера к клиенту
                string answer = "Server: OK";
                byte[] bytes_answer = Encoding.Unicode.GetBytes(answer);
                socket_client.Send(bytes_answer);
            }

            Console.ReadLine();
        }
    }
}
