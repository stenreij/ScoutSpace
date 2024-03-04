using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public enum Role
    {
        [Display(Name = "Eigenaar")]
        Owner,
        [Display(Name = "Admin")]
        Admin,
        [Display(Name = "Scout")]
        Scout
    }
}
