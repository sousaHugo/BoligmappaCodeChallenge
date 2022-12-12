namespace SecondApplication.Application.Dtos;

public class PostInfoDto
{
    public string Id { get; set; }
    public string PostId { get; set; }
    public string Username { get; set; }
    public bool HasFrenchTag { get; set; }
    public bool HasFictonTag { get; set; }
    public bool HasMoreThanTwoReactions { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}; PostId: {PostId}; Username: {Username}; French Tag: {HasFrenchTag}; Fiction Tag: {HasFictonTag};" +
            $" More Than Two Reactions: {HasMoreThanTwoReactions}; Created: {CreatedDate};" +
            $" Modified: {ModifiedDate}";
    }
}
