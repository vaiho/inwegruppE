using DoorinWebApp.Models;
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
        [DisplayName("Stad")]
        public string City { get; set; }
        [DisplayName("Födelsedatum")]
        public Nullable<System.DateTime> Birthdate { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string Username { get; set; }
        public string Fullname => $"{Firstname} {Lastname}";


        public List<competence> CompetencesList{ get; set; }
        public List<FullTechnology> TechnologysList { get; set; }

        public FreelancerProfileVM()
        {
            CompetencesList = new List<competence>();
            TechnologysList = new List<FullTechnology>();
            
        }
        [DisplayName("Ålder")]
        public int Age //För att räkna ut en ålder
        {
            get
            {
                DateTime dob = Convert.ToDateTime(Birthdate);
                int age = CalculateAge(dob);
                return age;
            }
        }
        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }
    }
}