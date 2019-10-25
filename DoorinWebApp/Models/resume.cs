namespace DoorinWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("resume")]
    public partial class resume
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public resume()
        {
            education = new HashSet<education>();
            workhistory = new HashSet<workhistory>();
            competence = new HashSet<competence>();
        }

        [Key]
        public int resume_id { get; set; }

        public int freelancer_id { get; set; }

        [StringLength(10)]
        public string driving_license { get; set; }

        [StringLength(2550)]
        public string profile { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<education> education { get; set; }

        public virtual freelancer freelancer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<workhistory> workhistory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<competence> competence { get; set; }
    }
}
