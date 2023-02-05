using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpChat_Library.Models
{
    public class Hero
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Weapon Weapon { get; set; }

        public Item[] Items { get; set; }
    }
}
