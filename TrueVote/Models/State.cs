using CsvHelper.Configuration.Attributes;

namespace TrueVote.Models
{
    public class State
    {
        [Ignore]
        public int Id { get; set; }
        [Name("COD_EDO")]
        public int Code { get; set; }
        [Name("EDO")]
        public string Name { get; set; }
        [Ignore]
        public List<Municipality> Municipalities { get; set; }
    }
}
