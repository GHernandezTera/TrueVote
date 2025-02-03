using CsvHelper;
using CsvHelper.Configuration;
using Hangfire;
using System.Globalization;
using TrueVote.Entities;
using TrueVote.Utilities;

namespace TrueVote.Jobs
{
    public class CsvUploader<T>
    {
        public int Page { get; private set; }
        public int PageSize { get; set; }
        public bool Ready { get; private set; }

        private IEnumerator<T>? records;

        public string FilePath { get; set; }

        public CsvUploader(string? filePath = null, int page = 0, int pageSize = 1000)
        {
            FilePath = filePath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "RESULTADOS_2024_CSV_V2.csv");
            Page = page;
            PageSize = pageSize;
        }
    }
}
