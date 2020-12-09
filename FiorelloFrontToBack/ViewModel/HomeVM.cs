using FiorelloFrontToBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloFrontToBack.ViewModel
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public SliderContent SliderContent { get; set; }
        public List<Category> Categories { get; set; }
        public About About { get; set; }
        public List<AboutSubtitleList> AboutSubtitleLists { get; set; }
        public List<SectionHeader> SectionHeaders { get; set; }
        public List<Expert> Experts { get; set; }
        public List<Blog> Blogs { get; set; }


    }
}
