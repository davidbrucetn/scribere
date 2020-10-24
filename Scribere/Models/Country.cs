using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scribere.Models
{
    public class Country
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
