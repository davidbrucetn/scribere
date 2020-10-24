using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scribere.Models
{
    public class UserImage
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [MaxLength(300)]
        public string ImageUrl { get; set; }
    }
}
