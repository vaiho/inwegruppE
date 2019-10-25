namespace DoorinWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("workhistory")]
    public partial class workhistory
    {
        [Key]
        public int workhistory_id { get; set; }

        public int resume_id { get; set; }

        [StringLength(50)]
        public string employer { get; set; }

        [StringLength(25)]
        public string position { get; set; }

        [StringLength(255)]
        public string description { get; set; }

        [StringLength(25)]
        public string date { get; set; }

        public virtual resume resume { get; set; }
    }
}
