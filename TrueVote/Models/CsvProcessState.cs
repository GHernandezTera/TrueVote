namespace TrueVote.Models
{
    public class CsvProcessState
    {
        public int Id { get; set; }
        public int LastProcessedRow { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
