
namespace FirstApplication.Application.Dtos;

public class GetAllUserInfoDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Username { get; set; }
    public int NumberOfPosts { get; set; }
    public int NumberOfTodos { get; set; }
    public bool UseMasterCard { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}; UserId: {UserId}; Username: {Username}; Number of Posts: {NumberOfPosts};" +
            $" Number of Todos: {NumberOfTodos}; Master Card: {UseMasterCard};" +
            $" Created: {CreatedDate};" +
            $" Modified: {ModifiedDate}";
    }
}
