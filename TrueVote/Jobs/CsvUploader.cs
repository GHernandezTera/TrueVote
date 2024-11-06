using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using TrueVote.Utilities;

namespace TrueVote.Jobs
{
    public class CsvUploader<T>(string filePath = @"~/data/RESULTADOS_2024_CSV_V2.csv")
    {
        public int Page { get; private set; } = 0;
        public int PageSize { get; set; } = 1000;
        public bool Ready { get; private set; } = false;

        private IEnumerator<T> records;

        public string FilePath { get; } = filePath;

        public bool Prepare()
        {
            Page = 0;
            try
            {
                if (File.Exists(FilePath))
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture);
                    using (StreamReader streamReader = new StreamReader(filePath))
                    using (CsvReader csvReader = new CsvReader(streamReader, config))
                    {
                        var enumerable = csvReader.GetRecords<T>();

                        records = enumerable.GetEnumerator();

                        ConsoleUtilities.Sucess("File ready to process!");

                        Ready = true;
                    }
                }
            }
            catch (FileNotFoundException fnfEx)
            {
                ConsoleUtilities.Error("The file couldn´t be found!", fnfEx);
            }
            catch (Exception ex)
            {
                ConsoleUtilities.Error("Something unexpected went wrong!", ex);
            }
            return Ready;
        }

        public void Process()
        {
            if (!Ready)
            {
                ConsoleUtilities.Error("File is not ready!");
                return;
            }

            for (int i = 0; i < PageSize && records.MoveNext(); i++)
            {
                T results = records.Current;
            }
        }
    }                                                          
}
