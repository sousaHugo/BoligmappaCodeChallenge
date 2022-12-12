namespace FirstApplication.Application.Dtos;

public class TodoDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}; Title: {Title}; Description: {Description}; UserId: {UserId}";
    }
}
