namespace BCCP.DummyApi.Models
{
    public class PostModel
    {
        public string Id { get; set; }
        public string Post { get; set; }
        public string Username { get; set; }
        public IEnumerable<string> Tags { get; set; } = new List<string>();
        public IEnumerable<string> Reactions { get; set; } = new List<string>();
    }
}
