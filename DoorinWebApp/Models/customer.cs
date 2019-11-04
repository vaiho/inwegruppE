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

    public partial class customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public customer()
        {
            this.freelancer = new HashSet<freelancer>();
        }
    
        public int customer_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        [DisplayName("Telefonnummer")]
        [Required(ErrorMessage = "V�nligen fyll i ett telefonnummer")]
        public string phonenumber { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string position { get; set; }
        public string company { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<freelancer> freelancer { get; set; }
    }
}
