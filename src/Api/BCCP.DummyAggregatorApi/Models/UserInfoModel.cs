namespace BCCP.DummyAggregatorApi.Models;

public class UserInfoModel
{
    public string Id { get; set; }
    public string Username { get; set; }
    public CardType CardType { get; set; }
    public List<PostModel> Posts { get; set; } = new List<PostModel>();
    public List<TodoModel> Todos { get; set; } = new List<TodoModel>();

}

public enum CardType
{
    VISA,
    MASTERCARD
}
