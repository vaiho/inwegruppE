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
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public string phonenumber { get; set; }
        public string email { get; set; }
        public Nullable<System.DateTime> birthdate { get; set; }
        public string birthcity { get; set; }
        public string nationality { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public IEnumerable<SelectListItem> country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<customer> customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<resume> resume { get; set; }
    }
}
