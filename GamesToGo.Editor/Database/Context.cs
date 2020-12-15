using GamesToGo.Editor.Project;
using GamesToGo.Editor.Database.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using osu.Framework.Testing;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GamesToGo.Editor.Database
{
    [ExcludeFromDynamicCompile]
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
                project.Property(e => e.Description).IsRequired();
                project.Property(e => e.CreatorID).IsRequired();
                project.Property(e => e.MinNumberPlayers).IsRequired();
                project.Property(e => e.MaxNumberPlayers).IsRequired();
                project.Property(e => e.NumberCards).IsRequired();
                project.Property(e => e.NumberTokens).IsRequired();
                project.Property(e => e.NumberBoxes).IsRequired();
                project.Property(e => e.NumberBoards).IsRequired();
                project.Property(e => e.OnlineProjectID);
                project.Property(e => e.CommunityStatus).IsRequired();
                project.Property(e => e.ImageRelationID);
                project.Property(e => e.Tags);
            });

            modelBuilder.Entity<FileRelation>().HasKey(fr => fr.RelationID);
            modelBuilder.Entity<FileRelation>(relation =>
            {
                relation.Property(e => e.RelationID).IsRequired();
                relation.Property(e => e.ProjectID).IsRequired();
                relation.Property(e => e.FileID).IsRequired();
            });

            modelBuilder.Entity<FileRelation>().HasOne(fr => fr.Project).WithMany(p => p.Relations).HasForeignKey(fr => fr.ProjectID);
            modelBuilder.Entity<FileRelation>().HasOne(fr => fr.File).WithMany(f => f.Relations).HasForeignKey(fr => fr.FileID);
            modelBuilder.Entity<ProjectInfo>().HasOne(pi => pi.File).WithOne(f => f.Project).HasForeignKey<ProjectInfo>(pi => pi.FileID);
            modelBuilder.Entity<ProjectInfo>().HasOne(pi => pi.ImageRelation).WithOne().HasForeignKey<ProjectInfo>(pi => pi.ImageRelationID);
        }
    }
}
