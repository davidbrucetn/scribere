using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scribere.Models
{
    public class FavoriteAuthor
    {
        public int Id {get;set;}
        public int SourceUserId { get; set; }
        public int FavoriteUserId { get; set; }
    }
}
