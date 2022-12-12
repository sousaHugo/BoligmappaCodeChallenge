namespace FirstApplication.Application.Dtos.Extensions;

public static class UserInfoAggregatorDtoExtensions
{
    public static UserInfoDto AsUserInfoDto(this UserInfoAggregatorDto Dto)
    {
        if (Dto is null)
            return null;

        return new UserInfoDto()
        {
            UserId = Dto.Id,
            UseMasterCard = Dto.CardType == CardType.MASTERCARD,
            NumberOfPosts = Dto.Posts.Count,
            NumberOfTodos = Dto.Todos.Count,
            Username = Dto.Username
        };
    }

    public static IEnumerable<UserInfoDto> AsUserInfoDto(this IEnumerable<UserInfoAggregatorDto> Dto)
    {
        if (Dto is null)
            return new List<UserInfoDto>();

        return Dto.Select(a => a.AsUserInfoDto());
    }
}
