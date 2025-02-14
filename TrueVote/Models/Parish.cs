using CsvHelper.Configuration.Attributes;

namespace TrueVote.Models
{
    public class Parish
    {
        [Ignore]
        public int Id { get; set; }
        [Name("COD_PAR")]
        public int Code { get; set; }
        [Name("PAR")]
        public string Name { get; set; }
        [Name("COD_MUN")]
        public int MunicipalityCode { get; set; }
        [Ignore]
        public Municipality Municipality { get; set; }
    }
}
