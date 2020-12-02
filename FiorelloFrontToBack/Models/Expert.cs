using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloFrontToBack.Models
{
    public class Expert
    {
        public int Id { get; set; }
        [Required,StringLength(150)]
        public string Image { get; set; }
        [Required, StringLength(150)]
        public string Name { get; set; }
        public string Position { get; set; }
        public string Info { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTime { get; set; }
    }
}
