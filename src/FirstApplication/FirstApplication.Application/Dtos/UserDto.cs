namespace FirstApplication.Application.Dtos;

public class UserDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public CardType CardType { get; set; }
}

public enum CardType
{
    VISA,
    MASTERCARD
}
