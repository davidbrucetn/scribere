using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scribere.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        [MaxLength(50)]
        public string Subject { get; set; }
        [MaxLength(300)]
        public string Content { get; set; }

        public int IsDeleted { get; set; }
    }
}
