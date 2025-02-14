using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using TrueVote.Models;

namespace TrueVote.Services
{
    public class CsvService
    {
        private readonly ILogger<CsvService> _logger;

        public CsvService(ILogger<CsvService> logger)
        {
            _logger = logger;
        }

        public List<VotingRecord> getData(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
            };

            try
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, config);
                var votingRecords = new List<VotingRecord>();

                while(csv.Read())
                {
                    var state = csv.GetRecord<State>();
                    var mun = csv.GetRecord<Municipality>();
                    var par = csv.GetRecord<Parish>();
                    var record = csv.GetRecord<VotingRecord>();
                    record.State = state;
                    record.Municipality = mun;
                    record.Parish = par;

                    votingRecords.Add(record);
                }

                return votingRecords;
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, $"Error: El archivo {filePath} no fue encontrado.");
            }
            catch (CsvHelperException ex)
            {
                _logger.LogError(ex, "Error al leer el archivo CSV o mapear los datos.");
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "Error de lectura/escritura.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado.");
            }

            return new List<VotingRecord>();
        }
    }
}
