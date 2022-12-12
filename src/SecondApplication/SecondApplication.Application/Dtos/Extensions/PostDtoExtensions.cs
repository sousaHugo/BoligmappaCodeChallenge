namespace SecondApplication.Application.Dtos.Extensions;

public static class PostDtoExtensions
{
    public static PostInfoDto AsPostInfoDto(this PostDto PostDto)
    {
        if (PostDto is null)
            return null;

        return new PostInfoDto()
        {
            PostId = PostDto.Id,
            HasFictonTag = PostDto.Tags != null && PostDto.Tags.Any(a => a == "FICTION"),
            HasFrenchTag = PostDto.Tags != null && PostDto.Tags.Any(a => a == "FRENCH"),
            HasMoreThanTwoReactions = PostDto.Tags != null && PostDto.Tags.Count() > 2,
            Username = PostDto.Username
        };
    }
    public static IEnumerable<PostInfoDto> AsPostInfoDto(this IEnumerable<PostDto> PostDtoList)
    {
        if (PostDtoList is null)
            return new List<PostInfoDto>();

        return PostDtoList.Select(a => a.AsPostInfoDto());
    }
}
