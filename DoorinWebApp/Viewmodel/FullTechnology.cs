using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoorinWebApp.Viewmodel
{
    public class FullTechnology
    {
        public int technology_id { get; set; }
        public string name { get; set; }
        public int resume_id { get; set; }
        public int competence_id { get; set; } //Vilken kompetens den tillhör
        public Nullable<bool> core_technology { get; set; }
        public Nullable<int> rank { get; set; }
    }
}