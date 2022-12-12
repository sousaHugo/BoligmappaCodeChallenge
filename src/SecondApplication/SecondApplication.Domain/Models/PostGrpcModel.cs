
namespace SecondApplication.Domain.Models;

public class PostGrpcModel
{
    public string Id { get; set; }
    public string Post { get; set; }
    public string Username { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public IEnumerable<string> Reactions { get; set; }
}
