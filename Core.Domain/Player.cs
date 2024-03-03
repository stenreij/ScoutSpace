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
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime birthDate { get; set; }
        public string? residence { get; set; }
        public string? email { get; set; }
        public int? phoneNr { get; set; }
        public Line? line { get; set; }
        public Position? position { get; set; }
        public Foot? preferedFoot { get; set; }
        public int? teamId { get; set; }
        public Team? team { get; set; }
        public ICollection<Note>? notities { get; set; }
    }
}
