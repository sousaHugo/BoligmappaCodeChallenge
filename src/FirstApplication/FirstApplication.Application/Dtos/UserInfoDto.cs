namespace FirstApplication.Application.Dtos;

public class UserInfoDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Username { get; set; }
    public int NumberOfPosts { get; set; }
    public int NumberOfTodos { get; set; }
    public bool UseMasterCard { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
