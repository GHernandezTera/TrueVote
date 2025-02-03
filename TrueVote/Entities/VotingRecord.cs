using CsvHelper.Configuration.Attributes;

namespace TrueVote.Entities
{
    public class VotingRecord
    {
        public int Id { get; set; }
        [Name("COD_EDO")]
        public int StateId { get; set; }
        public virtual State State { get; set; }
        [Name("COD_MUN")]
        public int MunicipalityId { get; set; }
        public virtual Municipality Municipality { get; set; }
        [Name("COD_PAR")]
        public int ParishId { get; set; }
        public virtual Parish Parish { get; set; }
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
