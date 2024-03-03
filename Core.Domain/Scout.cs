using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Scout
    {
        public int scoutId {  get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public Role role { get; set; }
        public DateTime birthDate { get; set; }
        public string email { get; set; }
        public int phoneNr { get; set; }

    }
}
