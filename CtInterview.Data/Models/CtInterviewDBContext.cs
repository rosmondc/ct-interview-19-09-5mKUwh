using Microsoft.EntityFrameworkCore;

namespace Ct.Interview.Data.Models
{
    public partial class CtInterviewDBContext : DbContext
    {
        public CtInterviewDBContext()
        {
        }

        public CtInterviewDBContext(DbContextOptions<CtInterviewDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AsxListedCompany> AsxListedCompany { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=CtInterviewDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<AsxListedCompany>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AsxCode).HasMaxLength(5);

                entity.Property(e => e.CompanyName).HasMaxLength(250);

                entity.Property(e => e.GicsIndustryGroup).HasMaxLength(250);
            });
        }
    }
}
