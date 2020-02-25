using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace GamesToGo.Desktop.Database.Models
{
    public class Context : DbContext
    {
        // This property defines the table
        private string connectionString { get {
                var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "DesktopDatabase.db" };
                return connectionStringBuilder.ToString();
            } }
        public System.Data.Entity.DbSet<File> Files { get; set; }
        public System.Data.Entity.DbSet<ProyectInfo> Proyects { get; set; }
        public System.Data.Entity.DbSet<FileRelation> Relations { get; set; }

        // This method connects the context with the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "DesktopDatabase.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>().HasKey(f => f.FileID);
            modelBuilder.Entity<File>(file =>
            {
                file.Property(e => e.FileID).IsRequired();
                file.Property(e => e.OriginalName).IsRequired();
                file.Property(e => e.Type).IsRequired();
                file.Property(e => e.NewName).IsRequired().HasMaxLength(40);
            });

            modelBuilder.Entity<ProyectInfo>().HasKey(p => p.LocalProyectID);
            modelBuilder.Entity<ProyectInfo>(proyect =>
            {
                proyect.Property(e => e.LocalProyectID).IsRequired();
                proyect.Property(e => e.Name).IsRequired();
                proyect.Property(e => e.CreatorID).IsRequired();
                proyect.Property(e => e.NumberPlayers).IsRequired();
                proyect.Property(e => e.NumberCards).IsRequired();
                proyect.Property(e => e.NumberTokens).IsRequired();
                proyect.Property(e => e.NumberBoxes).IsRequired();
                proyect.Property(e => e.OnlineProyecrID);
                proyect.Property(e => e.ModerationStatus).IsRequired();
                proyect.Property(e => e.ComunityStatus).IsRequired();
            });

            modelBuilder.Entity<FileRelation>().HasKey(fr => new { fr.FileID, fr.ProyectID });
            modelBuilder.Entity<FileRelation>().HasOne(fr => fr.Proyect).WithMany(p => p.Relations).HasForeignKey(fr => fr.ProyectID);
            modelBuilder.Entity<FileRelation>().HasOne(fr => fr.File).WithMany(f => f.Relations).HasForeignKey(fr => fr.FileID);
        }
    }
}
