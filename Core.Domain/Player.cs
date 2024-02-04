using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Player
    {
        public int playerId { get; set; }
        public string name { get; set; }
        public string residence { get; set; }
        public string email { get; set; }
        public int phoneNr { get; set; }
        public Line line { get; set; }
        public Position position { get; set; }
    }
}
