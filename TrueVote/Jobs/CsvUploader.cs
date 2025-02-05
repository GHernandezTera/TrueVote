using Hangfire;
using Microsoft.EntityFrameworkCore;
using TrueVote.Database;
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

        public async Task processFile(string filePath)
        {
            try
            {
                var progress = await _context.CsvProcessState.FirstOrDefaultAsync();
                var lastProcessedRow = progress?.LastProcessedRow ?? 0;

                var data = _csvService.getData(filePath);
                var totalRecords = data.Count;

                // Si ya se han procesado todos los registros, detiene el job recurrente
                if (lastProcessedRow >= totalRecords)
                {
                    RecurringJob.RemoveIfExists("csv-processing");
                    ConsoleUtilities.Sucess("Processing completed.");
                    return;
                }

                if (lastProcessedRow < totalRecords)
                {
                    var batch = data.Skip(lastProcessedRow).Take(_batchSize).ToList();

                    if (batch.Count > 0)
                    {
                        await _dbService.saveResultsAsync(batch);

                        if (progress == null)
                        {
                            _context.CsvProcessState.Add(new CsvProcessState
                            {
                                LastProcessedRow = lastProcessedRow + batch.Count,
                                UpdatedAt = DateTime.UtcNow
                            });
                        }
                        else
                        {
                            progress.LastProcessedRow = lastProcessedRow + batch.Count;
                            progress.UpdatedAt = DateTime.UtcNow;
                        }

                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado durante el procesamiento del CSV.");
                RecurringJob.RemoveIfExists("csv-processing");
            }
        }
    }
}
