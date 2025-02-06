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

                return csv.GetRecords<VotingRecord>().ToList();
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex.Message, $"Error: El archivo {filePath} no fue encontrado.");
            }
            catch (CsvHelperException ex)
            {
                _logger.LogError(ex.Message, "Error al leer el archivo CSV o mapear los datos.");
            }
            catch (IOException ex)
            {
                _logger.LogError(ex.Message, "Error de lectura/escritura.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Ocurrió un error inesperado.");
            }

            return new List<VotingRecord>();
        }
    }
}
