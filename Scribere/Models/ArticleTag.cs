using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scribere.Models
{
    public class ArticleTag
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int TagId { get; set; }
    }
}
