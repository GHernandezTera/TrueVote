namespace TrueVote.Models
{
    public class Parish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MunicipalityId { get; set; }
        public virtual Municipality Municipality { get; set; }
    }
}
