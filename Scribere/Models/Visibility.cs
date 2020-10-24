using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Scribere.Models
{
    public class Visibility
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
    }
}
