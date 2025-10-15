using Microsoft.EntityFrameworkCore;
using spi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace spi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<spi.Models.Area> Areas { get; set; }

        public DbSet<spi.Models.Evidencias> Evidencias { get; set; }
        public DbSet<spi.Models.Observaciones> Observaciones { get; set; }
        public DbSet<spi.Models.Proyecto> Proyectos { get; set; }
        public DbSet<spi.Models.Usuario> Usuarios { get; set; }

        public DbSet<spi.Models.ObservacionArea> ObservacionAreas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Proyecto>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.des_proy)
                      .IsRequired()
                      .HasMaxLength(200);

                // Relación uno a muchos
                entity.HasMany(p => p.Observaciones)
                      .WithOne(a => a.Proyecto)
                      .HasForeignKey(a => a.ProyectoId)
                      .OnDelete(DeleteBehavior.Cascade); // si borras el proyecto, se borran sus áreas
            });

            modelBuilder.Entity<Observaciones>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.des_obs)
                      .IsRequired()
                      .HasMaxLength(200);
            });


            modelBuilder.Entity<ObservacionArea>()
    .HasKey(oa => new { oa.ObservacionesId, oa.AreaId });

            modelBuilder.Entity<ObservacionArea>()
                .HasOne(oa => oa.Observaciones)
                .WithMany(o => o.ObservacionAreas)
                .HasForeignKey(oa => oa.ObservacionesId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ObservacionArea>()
                .HasOne(oa => oa.Area)
                .WithMany(a => a.ObservacionAreas)
                .HasForeignKey(oa => oa.AreaId)
                .OnDelete(DeleteBehavior.Restrict);


        }





    }
    }

   
