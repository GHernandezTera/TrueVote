using CsvHelper.Configuration.Attributes;

namespace TrueVote.Models
{
    public class VotingRecord
    {
        [Ignore]
        public int Id { get; set; }
        [Name("COD_EDO")]
        public int StateId { get; set; }
        [Name("EDO")]
        public string State { get; set; }
        [Name("COD_MUN")]
        public int MunicipalityId { get; set; }
        [Name("MUN")]
        public string Municipality { get; set; }
        [Name("COD_PAR")]
        public int ParishId { get; set; }
        [Name("PAR")]
        public string Parish { get; set; }
        [Name("CENTRO")]
        public int VotingCenter { get; set; }
        [Name("MESA")]
        public ushort VotingTable { get; set; }
        [Name("VOTOS_VALIDOS")]
        public int ValidVotes { get; set; }
        [Name("VOTOS_NULOS")]
        public int NullVotes { get; set; }
        [Name("EG")]
        public int EgVotes { get; set; }
        [Name("NM")]
        public int NmVotes { get; set; }
        [Name("LM")]
        public int LmVotes { get; set; }
        [Name("JABE")]
        public int JabeVotes { get; set; }
        [Name("JOBR")]
        public int JobrVotes { get; set; }
        [Name("AE")]
        public int AeVotes { get; set; }
        [Name("CF")]
        public int CfVotes { get; set; }
        [Name("DC")]
        public int DcVotes { get; set; }
        [Name("EM")]
        public int EmVotes { get; set; }
        [Name("BERA")]
        public int BeraVotes { get; set; }
        [Name("URL")]
        public string Url { get; set; }
    }
}
