using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Scribere.Models
{
    public class Book
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }

        [MaxLength(13)]
        public string ISBN { get; set; }
        [MaxLength(12)]
        public string LCCN { get; set; }
        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Author { get; set; }

    }
}
