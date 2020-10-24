﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scribere.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
    }
}
