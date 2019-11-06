using DoorinWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoorinWebApp.Viewmodel
{
    public class FullResume
    { 
        public int Resume_id { get; set; }
        public int Freelancer_id { get; set; }
        public string Driving_license { get; set; }
        public string Profile { get; set; }
        public List<competence> Competences { get; set; }
        public List<competence> MyCompetences { get; set; }
        public List<technology> Technologies { get; set; }
        public List<FullTechnology> MyTechnologies { get; set; }
        public List<links> Link { get; set; }
        public List<education> MyEducations { get; set; }
        public List<workhistory> MyWorkhistory { get; set; }
        public string Linkname { get; set; }
        public string Url { get; set; }
        public string Employer { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
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
        public int SelectedCompetenceId { get; set; }

        public IEnumerable<SelectListItem> DrivingLicenceChoice { get; set; } //För att skapa en lista att välja "Ja" eller "nej" på körkort


        public FullResume()
        {
            Competences = new List<competence>();
            MyCompetences = new List<competence>();
            Technologies = new List<technology>();
            MyTechnologies = new List<FullTechnology>();
            Link = new List<links>();
            MyWorkhistory = new List<workhistory>();

    }

    }

}