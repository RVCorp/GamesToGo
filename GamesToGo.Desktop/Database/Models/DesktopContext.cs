using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace GamesToGo.Desktop.Database.Models
{
    public class DesktopContext : DbContext
    {
        // This property defines the table
        public System.Data.Entity.DbSet<File> Files { get; set; }
        public System.Data.Entity.DbSet<Proyect> Proyects { get; set; }
        public System.Data.Entity.DbSet<Relation> Relations { get; set; }

        // This method connects the context with the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "DesktopDatabase.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }
    }
}
