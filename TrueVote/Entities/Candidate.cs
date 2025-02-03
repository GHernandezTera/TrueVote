namespace TrueVote.Entities
{
    public class Candidate(string code, string name)
    {
        public int Id { get; private set; }
        public string Code { get; private set; } = code;
        public string Name { get; private set; } = name;
    }
}
