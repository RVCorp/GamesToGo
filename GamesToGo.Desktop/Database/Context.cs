using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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
    }
}
