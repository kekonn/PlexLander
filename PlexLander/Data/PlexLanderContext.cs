using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlexLander.Models;

namespace PlexLander.Data
{
    public class PlexLanderContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public PlexLanderContext(DbContextOptions<PlexLanderContext> options) : base(options)
        {
        }

        public DbSet<App> Apps { get; set; }
        public DbSet<PlexAuthentication> PlexSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<App>().ToTable("Apps");
            modelBuilder.Entity<PlexAuthentication>().ToTable("PlexSessions")
                .HasMany<PlexServer>();
        }
    }

    public static class DbInitializer
    {
        public static void Initialize(PlexLanderContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
