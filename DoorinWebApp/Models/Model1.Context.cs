﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class doorinDBEntities : DbContext
    {
        public doorinDBEntities()
            : base("name=doorinDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<customer> customer { get; set; }
        public virtual DbSet<freelancer> freelancer { get; set; }
        public virtual DbSet<competence> competence { get; set; }
        public virtual DbSet<education> education { get; set; }
        public virtual DbSet<resume> resume { get; set; }
        public virtual DbSet<technology> technology { get; set; }
        public virtual DbSet<workhistory> workhistory { get; set; }
        public virtual DbSet<links> links { get; set; }
        public virtual DbSet<technology_resume> technology_resume { get; set; }
    }
}
