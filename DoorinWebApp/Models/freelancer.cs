//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoorinWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class freelancer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public freelancer()
        {
            this.customer = new HashSet<customer>();
            this.resume = new HashSet<resume>();
        }
    
        public int freelancer_id { get; set; }
        [DisplayName("F�rnamn")]
        [Required(ErrorMessage = "V�nligen fyll i ett f�rnamn")]
        public string firstname { get; set; }
        [DisplayName("Efternamn")]
        [Required(ErrorMessage = "V�nligen fyll i ett efternamn")]
        public string lastname { get; set; }
        [DisplayName("Adress")]
        public string address { get; set; }
        [DisplayName("Ort")]
        public string city { get; set; }
        [DisplayName("Postkod")]
        public string zipcode { get; set; }
        [DisplayName("Telefonnummer")]
        public string phonenumber { get; set; }
        [DisplayName("Epostadress")]
        [Required(ErrorMessage = "V�nligen fyll i en epostadress")]
        public string email { get; set; }
        [DisplayName("F�delsedatum")]
        public Nullable<System.DateTime> birthdate { get; set; }
        [DisplayName("F�delseort")]
        public string birthcity { get; set; }
        [DisplayName("Nationalitet")]
        [Required(ErrorMessage = "V�nligen ange nationalitet")]
        public string nationality { get; set; }
        [DisplayName("Anv�ndarnamn")]
        [Required(ErrorMessage = "V�nligen fyll i ett anv�ndarnamn")]
        public string username { get; set; }
        [DisplayName("L�senord")]
        [Required(ErrorMessage = "V�nligen fyll i ett l�senord")]
        public string password { get; set; }
        [DisplayName("Namn")]
        public string fullname => $"{firstname} {lastname}";
        [DisplayName("Nationalitet")]
        public IEnumerable<SelectListItem> country { get; set; } //F�r att skapa en lista av valbara nationalitet

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<customer> customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<resume> resume { get; set; }

    }
}
