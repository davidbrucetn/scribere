using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scribere.Models
{
    public class ArticleImage
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }

        [MaxLength(300)]
        public string ImageUrl { get; set; }
    }
}
