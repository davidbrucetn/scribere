using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scribere.Models
{
    public class MsgReceiver
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public int Read { get; set; }
        public int IsDeleted { get; set; }
    }
}
