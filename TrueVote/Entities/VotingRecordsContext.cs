﻿//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TrueVote.Entities
{
    public class VotingRecordsContext: DbContext
    {
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Parish> Parishes { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<VotingRecord> VotingRecords { get; set; }
        public VotingRecordsContext(DbContextOptions<VotingRecordsContext> options)
            : base(options)
        {
        }
    }
}
