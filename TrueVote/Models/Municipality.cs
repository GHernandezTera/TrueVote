using CsvHelper.Configuration.Attributes;

namespace TrueVote.Models
{
    public class Municipality
    {
        [Ignore]
        public int Id { get; set; }
        [Name("COD_MUN")]
        public int Code { get; set; }
        [Name("MUN")]
        public string Name { get; set; }
        [Name("COD_EDO")]
        public int StateCode { get; set; }
        [Ignore]
        public State State { get; set; }
        [Ignore]
        public List<Parish> Parishes { get; set; }
    }
}
