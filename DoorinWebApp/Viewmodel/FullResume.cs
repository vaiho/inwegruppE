using DoorinWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoorinWebApp.Viewmodel
{
    public class FullResume
    { 
        public int Resume_id { get; set; }
        public int Freelancer_id { get; set; }
        [Key]
        public string Name { get; set; }
        public bool? Driving_license { get; set; }
        public string Profile { get; set; }
        public List<competence> Competences { get; set; }
        public List<competence> MyCompetences { get; set; }
        public List<technology> Technologies { get; set; }
        public List<FullTechnology> MyTechnologies { get; set; }
        public List<links> Link { get; set; }
        public string Linkname { get; set; }
        public string Url { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> Birthdate { get; set; }
        public string Birthcity { get; set; }
        public string Nationality { get; set; }

        public FullResume()
        {
            Competences = new List<competence>();
            MyCompetences = new List<competence>();
            Technologies = new List<technology>();
            MyTechnologies = new List<FullTechnology>();
            Link = new List<links>();
        }

    }

}