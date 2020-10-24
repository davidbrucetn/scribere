using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scribere.Models
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(75)]
        public string Type { get; set; }
    }
}
