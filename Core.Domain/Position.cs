using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public enum Position
    {
        [Display(Name = "Keeper")]
        Keeper,
        [Display(Name = "Rechtsback")]
        Rechtsback,
        [Display(Name = "Centrale Verdediger")]
        CentraleVerdediger,
        [Display(Name = "Linksback")]
        Linksback,
        [Display(Name = "Verdedigende Middenvelder")]
        VerdedigendeMiddenvelder,
        [Display(Name = "Aanvallende Middenvelder")]
        AanvallendeMiddenvelder,
        [Display(Name = "Rechtsbuiten")]
        Rechtsbuiten,
        [Display(Name = "Valse Spits")]
        ValseSpits,
        [Display(Name = "Spits")]
        Spits,
        [Display(Name = "Linksbuiten")]
        Linksbuiten
    }
}
