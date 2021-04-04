using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Contexts
{
    public class FanDbContext : DbContext
    {
        public FanDbContext(DbContextOptions<FanDbContext> options) : base(options)
        {
        }

        //DbSets for entities
        public DbSet<Fan> Fans { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Setting Models for entity framework understading

            modelBuilder.Entity<Fan>(entity =>
            {
                entity.ToTable("Fans");
                entity.HasKey(e => e.FanId);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime2");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime2");
            });

        }
    }
}
