using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Wake.EF
{
    public class WakeContext: DbContext
    {
        public DbSet<NetworkInterface> NetworkInterfaces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"DataSource={GetDbPath()}");
        }
        
        private static string GetDbPath()
        {
            var roamingFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dbFolder = Path.Join(roamingFolder, "wake");
            Directory.CreateDirectory(dbFolder);
            return Path.Join(dbFolder, "wake.db");
        }
    }
}