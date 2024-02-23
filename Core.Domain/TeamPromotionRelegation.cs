using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class TeamPromotionRelegation
    {
        public int teamId { get; set; }
        public bool Promotion { get; set; }
        public bool Relegation { get; set; }
    }
}
