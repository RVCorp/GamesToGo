using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Project;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace GamesToGo.Desktop.Database.Models
{
    public class Context : DbContext
    {
        private readonly string connectionString;

        public DbSet<File> Files { get; set; }
        public DbSet<ProjectInfo> Projects { get; set; }
        public DbSet<FileRelation> Relations { get; set; }

        public Context() : this("DataSource=:memory:")
        {
            
        }

        public Context(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // This method connects the context with the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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

            modelBuilder.Entity<ProjectInfo>().HasKey(p => p.LocalProjectID);
            modelBuilder.Entity<ProjectInfo>(project =>
            {
                project.Property(e => e.LocalProjectID).IsRequired();
                project.Property(e => e.Name).IsRequired();
                project.Property(e => e.CreatorID).IsRequired();
                project.Property(e => e.MinNumberPlayers).IsRequired();
                project.Property(e => e.MaxNumberPlayers).IsRequired();
                project.Property(e => e.NumberCards).IsRequired();
                project.Property(e => e.NumberTokens).IsRequired();
                project.Property(e => e.NumberBoxes).IsRequired();
                project.Property(e => e.OnlineProjectID);
                project.Property(e => e.ModerationStatus).IsRequired();
                project.Property(e => e.ComunityStatus).IsRequired();
            });

            modelBuilder.Entity<FileRelation>().HasKey(fr => new { fr.FileID, fr.ProjectID });
            modelBuilder.Entity<FileRelation>().HasOne(fr => fr.Project).WithMany(p => p.Relations).HasForeignKey(fr => fr.ProjectID);
            modelBuilder.Entity<FileRelation>().HasOne(fr => fr.File).WithMany(f => f.Relations).HasForeignKey(fr => fr.FileID);
            modelBuilder.Entity<ProjectInfo>().HasOne(pi => pi.File).WithOne().HasForeignKey<ProjectInfo>(pi => pi.FileID);
        }
    }
}
