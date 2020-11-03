using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Scribere.Models
{
    public class Article
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public int UserId { get; set; }
        public UserData UserData { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ArticleImage ArticleImage { get; set; }
        [MaxLength(100)]
        public string Heading { get; set; }
        [MaxLength(40000)]
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }
        public int VisibilityId { get; set; }
        public Visibility Visibility { get; set; }


    }
}
