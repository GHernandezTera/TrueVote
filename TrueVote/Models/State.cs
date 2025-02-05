namespace TrueVote.Models
{
    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Municipality> Municipality { get; set; }
    }
}
