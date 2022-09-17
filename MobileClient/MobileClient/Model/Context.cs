using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace MobileClient.Model {
    public class Context : DbContext {
        public DbSet<GlobalSettings> GlobalSettings { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Message> Messages { get; set; }

        public Context() {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<GlobalSettings>().HasData(
                new GlobalSettings() { Id = 1 }
            );
            modelBuilder.Entity<Server>().HasData(
                new Server() {
                    Id = 1,
                    DisplayName = "Localserver",
                    Ip = "10.0.2.2",
                    Port = 25567
                }
            );
            modelBuilder.Entity<Contact>().Property(e => e.New).HasDefaultValue(false);
            modelBuilder.Entity<User>().Property(e => e.LastMessageId).HasDefaultValue(0);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "chatApp.db3");

            optionsBuilder
                .UseSqlite($"Filename={dbPath}");
        }
    }
}
