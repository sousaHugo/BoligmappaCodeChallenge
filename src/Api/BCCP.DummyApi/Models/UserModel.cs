namespace BCCP.DummyApi.Models;

public class UserModel
{
    public string Id { get; set; }
    public string Username { get; set; }
    public DateTime DateOfBirth { get; set; }
    public CardType CardType { get; set; }

}
public enum CardType
{
    VISA,
    MASTERCARD
}
