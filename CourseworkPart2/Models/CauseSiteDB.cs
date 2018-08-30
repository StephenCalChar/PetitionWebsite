using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CourseworkPart2.Models
{
    public class CauseSiteDB: DbContext
    {
        public CauseSiteDB(): base("CauseSiteDB")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CauseSiteDB>());
        }


        public DbSet<Cause> Causes { get; set;}
        public DbSet<Member> Members { get; set; }

        public DbSet<Signature> Signatures { get; set; }



          protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //sets up the relationships between the models. Sets PK for signature and its 2 FKs in the Cause and Member models.
            modelBuilder.Entity<Signature>()
                .HasKey(e => e.SignatureId); 
            modelBuilder.Entity<Signature>()
                .HasRequired(e => e.Member)
                .WithMany(c => c.SignedCauses)
                .HasForeignKey(e => e.MemberId);
            modelBuilder.Entity<Signature>()
                .HasRequired(e => e.Cause)
                .WithMany(c => c.CauseSignees)
                .HasForeignKey(e => e.CauseId);
                

        } 

    }
}