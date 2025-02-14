using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrueVote.Models;

namespace TrueVote.Database
{
    public class VotingRecordsContext : IdentityDbContext<User>
    {
        public DbSet<Parish> Parishes { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<VotingRecord> VotingRecords { get; set; }
        public DbSet<CsvProcessState> CsvProcessState { get; set; }
        public VotingRecordsContext(DbContextOptions<VotingRecordsContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Municipality>()
                .HasOne(m => m.State)
                .WithMany(s => s.Municipalities)
                .HasForeignKey(m => m.StateCode)
                .HasPrincipalKey(s => s.Code);

            modelBuilder.Entity<Parish>()
                .HasOne(p => p.Municipality)
                .WithMany(m => m.Parishes)
                .HasForeignKey(p => p.MunicipalityCode)
                .HasPrincipalKey(m => m.Code);

            modelBuilder.Entity<VotingRecord>()
                .HasOne(v => v.Parish)
                .WithMany()
                .HasForeignKey(v => v.ParishCode)
                .HasPrincipalKey(p => p.Code)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VotingRecord>()
                .HasOne(v => v.Municipality)
                .WithMany()
                .HasForeignKey(v => v.MunicipalityCode)
                .HasPrincipalKey(m => m.Code)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VotingRecord>()
                .HasOne(v => v.State)
                .WithMany()
                .HasForeignKey(v => v.StateCode)
                .HasPrincipalKey(s => s.Code)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
