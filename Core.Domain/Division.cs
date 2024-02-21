using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public enum Division
    {
        [Display(Name = "Eredivisie")]
        Eredivisie,
        [Display(Name = "Keukenkampioendivisie")]
        KeukenKampioenDivisie,
        [Display(Name = "Tweede Divisie")]
        TweedeDivisie,
        [Display(Name = "Derde Divisie")]
        DerdeDivisie,
        [Display(Name = "Vierde Divisie")]
        VierdeDivisie,
        [Display(Name = "Eerste Klasse")]
        EersteKlasse,
        [Display(Name = "Tweede Klasse")]
        TweedeKlasse,
        [Display(Name = "Derde Klasse")]
        DerdeKlasse,
        [Display(Name = "Vierde Klasse")]
        VierdeKlasse,
        [Display(Name = "Vijfde Klasse")]
        VijfdeKlasse
    }
}

