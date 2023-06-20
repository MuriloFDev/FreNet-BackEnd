using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using App.Model;

namespace App.Models
{
    public partial class _DbContext : DbContext
    {
        public DbSet<Quotation> Quotation { get; set; }
        public DbSet<CarrierAvaliable> CarrierAvaliable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                   .AddJsonFile("appsettings.json")
                   .Build();
           
                optionsBuilder.EnableSensitiveDataLogging(true);
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

            var connectionString = configuration.GetConnectionString("banco");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                opt => opt.CommandTimeout(int.MaxValue));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
              .HasCharSet("utf8");

            modelBuilder.Entity<Quotation>(entity =>
            {
                entity.HasKey(e => e.id)
                    .HasName("PRIMARY");

                entity.Property(e => e.ServiceCode)
                    .HasColumnType("text");

                entity.Property(e => e.ShippingPrice)
                    .HasColumnType("text");

                entity.Property(e => e.Carrier)
                    .HasColumnType("text");

                entity.Property(e => e.CarrierCode)
                    .HasColumnType("text");

                entity.Property(e => e.DeliveryTime)
                    .HasColumnType("text");

                entity.Property(e => e.Msg)
                   .HasColumnType("text");

                entity.Property(e => e.ServiceDescription)
                   .HasColumnType("text");

                entity.Property(e => e.OriginalDeliveryTime)
                   .HasColumnType("text");

                entity.Property(e => e.OriginalShippingPrice)
                   .HasColumnType("text");
            });

            modelBuilder.Entity<CarrierAvaliable>(entity =>
            {
                entity.HasKey(e => e.id)
                    .HasName("PRIMARY");
                
                entity.Property(e => e.Carrier)
                    .HasColumnType("text");

                entity.Property(e => e.CarrierCode)
                    .HasColumnType("text");

                entity.Property(e => e.ServiceCode)
                    .HasColumnType("text");

                entity.Property(e => e.ServiceDescription)
                    .HasColumnType("text");
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
