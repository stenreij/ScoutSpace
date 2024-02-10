using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public enum Line
    {
        [Display(Name = "Keeper")]
        Keeper,
        [Display(Name = "Verdediger")]
        Verdediger,
        [Display(Name = "Middenvelder")]
        Middenvelder,
        [Display(Name = "Aanvaller")]
        Aanvaller
    }
}
