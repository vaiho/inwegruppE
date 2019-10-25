namespace DoorinWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("technology")]
    public partial class technology
    {
        [Key]
        public int technology_id { get; set; }

        public int competence_id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public int rank { get; set; }

        public bool? core { get; set; }

        public virtual competence competence { get; set; }
    }
}
