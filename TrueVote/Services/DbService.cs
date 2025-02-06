using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
                var dbStates = new HashSet<int>(await _context.States
                    .Select(e => e.Id)
                    .ToListAsync());
                var newStates = data
                    .Select(r => new State { Id = r.StateId, Name = r.State })
                    .Distinct()
                    .Where(e => !dbStates.Contains(e.Id))
                    .ToList();


                if (newStates.Any())
                {
                    _context.States.AddRange(newStates);
                    await _context.SaveChangesAsync();
                }

                var dbMunicipalities = new HashSet<int>(await _context.Municipalities
                    .Select(m => m.Id)
                    .ToListAsync());
                var newMunicipalities = data
                    .Select(d => new Municipality { Name = d.Municipality, Id = d.MunicipalityId, StateId = d.StateId })
                    .Distinct()
                    .Where(m => !dbMunicipalities.Contains(m.Id))
                    .ToList();

                if (newMunicipalities.Any())
                {
                    _context.Municipalities.AddRange(newMunicipalities);
                    await _context.SaveChangesAsync();
                }

                var updatedMunicipalities = _context.Municipalities.Select(m => new Municipality
                {
                    Id = m.Id,
                    Name = m.Name,
                    StateId = m.StateId
                }).ToList();

                var dbParishes = new HashSet<int>(await _context.Parishes
                    .Select(p => p.Id)
                    .ToListAsync());
                var newParishes = data.Select(d => new Parish
                {
                    Name = d.Parish,
                    Id = d.ParishId,
                    MunicipalityId = d.MunicipalityId
                }).Distinct().Where(p => !dbParishes.Contains(p.Id)).ToList();

                if (newParishes.Any())
                {
                    _context.Parishes.AddRange(newParishes);
                    await _context.SaveChangesAsync();
                }

                _context.VotingRecords.AddRange(data);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message, $"Error updating the database.");
            }
            catch (IOException ex)
            {
                _logger.LogError(ex.Message, $"IO error.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Unexpected error ocurred.");
            }
        }
    }
}
