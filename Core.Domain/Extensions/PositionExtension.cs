using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Core.Domain.Extensions
{
    public static class PositionExtension
    {
        public static string GetDisplayName(this Enum value)
        {
            return value.GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DisplayAttribute>()
                ?.Name
                ?? value.ToString();
        }
    }
}
