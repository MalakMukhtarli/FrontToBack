﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloFrontToBack.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Image { get; set; }
    }
}
