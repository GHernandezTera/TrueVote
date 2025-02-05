using CsvHelper;
using CsvHelper.Configuration;
using Hangfire;
using System.Globalization;
using TrueVote.Models;
using TrueVote.Services;
using TrueVote.Utilities;

namespace TrueVote.Jobs
{
    public class CsvUploader(CsvService csvService, DbService dbService, VotingRecordsContext context,
                             ILogger<CsvUploader> logger)
    {
        private readonly CsvService _csvService = csvService;
        private readonly DbService _dbService = dbService;
        private readonly VotingRecordsContext _context = context;
        private readonly ILogger<CsvUploader> _logger = logger;
        private readonly int _batchSize = 5000;
    }
}
