using Microsoft.EntityFrameworkCore;
using PKiK.Server.DB.DBO;
using PKiK.Server.Shared;
using System;
using System.IO;
using System.Linq;

namespace PKiK.Server.DB {
    public class DataContext : DbContext {
        private static DataContext? instance;
        public DataContext() { instance = this;}
        public DataContext(DbContextOptions options) : base(options) { instance = this; }
        public static string? ReadConnectionString() {
            EnvironmentalSettings env = EnvironmentalSettings.Get();
            string[] cfg = File.ReadAllLines(env.AppSettingsPath+"//appsettings.json");
            string? connStr = null;
            foreach(var line in cfg) {
                if(line.Contains("DefaultConnection")) {
                    connStr = line.Split(':')[1];
                    connStr = connStr.Trim();
                    connStr = connStr.Trim('\"');
                    connStr = connStr.Replace("\\\\", "\\");
                    break;
                }
            }
            return connStr;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            AppDomain.CurrentDomain.SetData("DataDirectory", EnvironmentalSettings.Get().RootPath);
            Config config = Config.Get();
            string? connStr = null;
            if(config.ConnectionString == null) {
                connStr = ReadConnectionString();
            } else {
                connStr = config.ConnectionString;
            }
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connStr);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<MessageDBO>().HasOne(x => x.Sender).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MessageDBO>().HasMany(x => x.Recipients).WithOne().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<KeyDBO>().HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.Restrict);
        }

        public void CreateDefaultUsers() {
            if (!this.Users.Any(x => x.Username == "removed")) {
                this.Users.Add(new UserDBO() {
                    Username = "removed",
                    Name = "User",
                    Surname = "Removed",
                    Password = ""
                });
                this.SaveChanges();
            }
        }

        public DbSet<UserDBO> Users { get; set; }
        public DbSet<MessageDBO> Messages { get; set; }
        public DbSet<RecipientDBO> Recipients { get; set; }
        public DbSet<EventDBO> Events { get; set; }
        public DbSet<KeyDBO> Keys { get; set; }
        public DbSet<JwtDBO> Jwt { get; set; }
    }
}