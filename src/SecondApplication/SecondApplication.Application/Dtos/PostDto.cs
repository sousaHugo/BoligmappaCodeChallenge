namespace SecondApplication.Application.Dtos;

public class PostDto
{
    public string Id { get; set; }
    public string Post { get; set; }
    public string Username { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public IEnumerable<string> Reactions { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}; Post: {Post}; Username: {Username}; Tags: {Tags.Any()}; Reactions: {Reactions.Any()}";
    }

}
