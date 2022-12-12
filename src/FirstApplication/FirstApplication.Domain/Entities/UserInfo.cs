using BCCP.Shared.Entities;
using System.ComponentModel.DataAnnotations;

namespace FirstApplication.Domain.Entities;

public class UserInfo : EntityBase
{
    [Required]
    public string UserId { get; set; }
    [Required]
    public string Username { get; set; }
    public int NumberOfPosts { get; set; }
    public int NumberOfTodos { get; set; }
    public bool UseMasterCard { get; set; }
}
