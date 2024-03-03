using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Note
    {
        public int noteId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int playerId { get; set; }
    }
}
