using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloFrontToBack.Models
{
    public class AboutSubtitleList
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string Image { get; set; }
        [Required,StringLength(250)]
        public string Subtitle { get; set; }
    }
}
