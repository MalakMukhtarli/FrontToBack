using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloFrontToBack.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [Required, StringLength(150)]
        public string Image { get; set; }
        [Required, StringLength(150)]
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime AddTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTime { get; set; }
    }
}
