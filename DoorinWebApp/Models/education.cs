namespace DoorinWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("education")]
    public partial class education
    {
        [Key]
        public int education_id { get; set; }

        public int resume_id { get; set; }

        [StringLength(50)]
        public string title { get; set; }

        [StringLength(255)]
        public string description { get; set; }

        [StringLength(25)]
        public string date { get; set; }

        public virtual resume resume { get; set; }
    }
}
