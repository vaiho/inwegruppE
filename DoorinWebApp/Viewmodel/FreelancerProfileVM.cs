using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoorinWebApp.Viewmodel
{
    public class FreelancerProfileVM
    {
        [Key]
        public int Freelancer_id { get; set; }
        [DisplayName("Förnamn")]
        public string Firstname { get; set; }
        [DisplayName("Efternamn")]
        public string Lastname { get; set; }
        [DisplayName("Epost")]
        public string Email { get; set; }
        [DisplayName("Nationalitet")]
        public string Nationality { get; set; }
        [DisplayName("Id på CV")]
        public int Resume_id { get; set; }
        [DisplayName("Profil")]
        public string ProfileText { get; set; }

    }
}