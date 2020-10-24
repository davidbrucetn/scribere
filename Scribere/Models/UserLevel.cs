using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scribere.Models
{
    public class UserLevel
    {
        public int Id { get; set; }

        [MaxLength(25)]
        public string Level { get; set; }
    }
}
