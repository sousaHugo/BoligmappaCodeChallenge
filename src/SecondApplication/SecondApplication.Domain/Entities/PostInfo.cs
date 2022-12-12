using BCCP.Shared.Entities;

namespace SecondApplication.Domain.Entities;

public class PostInfo : EntityBase
{
    public string PostId { get; set; }
    public string Username { get; set; }
    public bool HasFrenchTag { get; set; }
    public bool HasFictonTag { get; set; }
    public bool HasMoreThanTwoReactions { get; set; }

}
