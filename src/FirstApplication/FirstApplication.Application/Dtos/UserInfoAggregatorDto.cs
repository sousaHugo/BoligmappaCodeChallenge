namespace FirstApplication.Application.Dtos;

public class UserInfoAggregatorDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public CardType CardType { get; set; }
    public List<PostDto> Posts { get; set; } = new List<PostDto>();
    public List<TodoDto> Todos { get; set; } = new List<TodoDto>();
}
