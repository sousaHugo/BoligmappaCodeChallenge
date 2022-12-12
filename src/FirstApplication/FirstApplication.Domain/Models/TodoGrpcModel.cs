namespace FirstApplication.Domain.Models;

public class TodoGrpcModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }
}
