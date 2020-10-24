using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scribere.Models
{
    public class FavoriteArticle
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }
    }
}
