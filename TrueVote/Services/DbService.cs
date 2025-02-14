using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TrueVote.Database;
using TrueVote.Models;

namespace TrueVote.Services
{
    public class DbService
    {
        private readonly VotingRecordsContext _context;

        private readonly ILogger<CsvService> _logger;

        public DbService(VotingRecordsContext context, ILogger<CsvService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task saveResultsAsync(List<VotingRecord> data)
        {
            try
            {
                _logger.LogInformation($"###### Saving {data.Count} records to the database.");

                var dbStates = new HashSet<int>(_context.States
                    .AsNoTracking()
                    .Select(e => e.Code)
                    .ToList());
                var newStates = data
                    .Where(d => d.State != null)
                    .Select(d => d.State!)
                    .Distinct()
                    .Where(s => !dbStates.Contains(s.Code))
                    .ToList();

                var dbMunicipalities = new HashSet<int>(_context.Municipalities
                    .AsNoTracking()
                    .Select(m => m.Code)
                    .ToList());
                var newMunicipalities = data
                    .Where(d => d.Municipality != null)
                    .Select(d => d.Municipality!)
                    .Distinct()
                    .Where(m => !dbMunicipalities.Contains(m.Code))
                    .ToList();

                var dbParishes = new HashSet<int>(await _context.Parishes
                    .AsNoTracking()
                    .Select(p => p.Code)
                    .ToListAsync());
                var newParishes = data
                    .Where(d => d.Parish != null)
                    .Select(d => d.Parish!)
                    .Distinct()
                    .Where(p => !dbParishes.Contains(p.Code))
                    .ToList();

                if (newStates.Any())
                {
                    _context.States.AddRange(newStates);
                }

                if (newMunicipalities.Any())
                {
                    _context.Municipalities.AddRange(newMunicipalities);
                }

                if (newParishes.Any())
                {
                    _context.Parishes.AddRange(newParishes);
                }

                data.ForEach(record =>
                {
                    record.State = null;
                    record.Municipality = null;
                    record.Parish = null;
                });

                _context.VotingRecords.AddRange(data);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"###### Saved {data.Count} records to the database.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Error updating the database.");
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, $"IO error.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error ocurred.");
            }
        }
    }
}
