//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrueVote.Models;

namespace TrueVote.Database
{
    public class VotingRecordsContext : DbContext
    {
        public DbSet<Candidate> Candidates { get; set; }
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
            base.OnModelCreating(modelBuilder); //This is necessary for Identity to function correctly
        }
    }
}
