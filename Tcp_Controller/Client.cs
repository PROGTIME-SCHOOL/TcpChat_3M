using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TcpChat_Library.Models;
using TcpChat_Library;

namespace Tcp_Controller
{
    public class Client
    {
        Hero hero = new Hero();
        Socket socket_sender;

        public Client() 
        {
            // game
            hero.Weapon = new Weapon();
            hero.Items = new Item[]
            {
                new Item() {Name = "Лапти", Description = "Нужны для прогулки"},
                new Item() {Name = "Плащ", Description = "Он зеленый"},
                new Item() {Name = "Апельсин", Description = "Для перекуса"}
            };

            // seti
            socket_sender = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            IPAddress address = IPAddress.Parse("127.0.0.1");  // server ip
            IPEndPoint endRemoutePoint = new IPEndPoint(address, 7632);   // server port

            socket_sender.Connect(endRemoutePoint);
        }

        public void MoveRight()
        {
            hero.X += 50;

            Utility.JsonSerializeAndSend(hero, socket_sender);
        }

        public void MoveLeft()
        {
            hero.X -= 50;

            Utility.JsonSerializeAndSend(hero, socket_sender);
        }

        public void MoveUp()
        {
            hero.Y -= 50;

            Utility.JsonSerializeAndSend(hero, socket_sender);
        }

        public void MoveDown()
        {
            hero.Y += 50;

            Utility.JsonSerializeAndSend(hero, socket_sender);
        }
    }
}
