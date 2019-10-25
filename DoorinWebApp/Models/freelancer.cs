namespace DoorinWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("freelancer")]
    public partial class freelancer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public freelancer()
        {
            resume = new HashSet<resume>();
        }

        [Key]
        public int freelancer_id { get; set; }

        [Required]
        [StringLength(25)]
        public string firstname { get; set; }

        [StringLength(50)]
        public string lastname { get; set; }

        [StringLength(50)]
        public string address { get; set; }

        [StringLength(25)]
        public string city { get; set; }

        [StringLength(25)]
        public string zipcode { get; set; }

        [StringLength(25)]
        public string phonenumber { get; set; }

        [Required]
        [StringLength(50)]
        public string email { get; set; }

        [Column(TypeName = "date")]
        public DateTime? birthdate { get; set; }

        [StringLength(25)]
        public string birthcity { get; set; }

        [StringLength(25)]
        public string nationality { get; set; }

        [Required]
        [StringLength(25)]
        public string username { get; set; }

        [Required]
        [StringLength(25)]
        public string password { get; set; }
        public IEnumerable<SelectListItem> country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<resume> resume { get; set; }
    }
}
