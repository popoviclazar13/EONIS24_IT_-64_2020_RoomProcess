using Microsoft.EntityFrameworkCore;
using RoomProcess.Models.Entities;

namespace RoomProcess.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Uloga> Uloga { get; set; }
        public DbSet<Korisnik> Korisnik { get; set; }
        public DbSet<Objekat> Objekat { get; set; }
        public DbSet<Oprema> Oprema { get; set; }
        public DbSet<Popust> Popust { get; set; }
        public DbSet<Recenzija> Recenzija { get; set; }
        public DbSet<Rezervacija> Rezervacija { get; set; }
        public DbSet<TipObjekta> TipObjekta { get; set; }
        //Za STRIPE
        public DbSet<Racun> Racun { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
