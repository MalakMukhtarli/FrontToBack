using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloFrontToBack.Models
{
    public class SectionHeader
    {
        public int Id { get; set; }
        [Required,StringLength(150)]
        public string Title { get; set; }
        [Required, StringLength(250)]
        public string Content { get; set; }
    }
}
