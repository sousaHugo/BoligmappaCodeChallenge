using SecondApplication.Domain.Entities;

namespace SecondApplication.Application.Contracts.Repositories;

public interface IPostInfoRepository
{
    /// <summary>
    /// This method is responsible for getting all the information about the posts on the database.
    /// </summary>
    /// <returns>IReadOnlyList<PostInfo> - List of PostInfo</returns>
    Task<IReadOnlyList<PostInfo>> GetAllAsync();

    /// <summary>
    /// This method is responsible for getting the Post Information related wit the PostId sended by parameter.
    /// </summary>
    /// <param name="PostId">Post Information to find.</param>
    /// <returns>PostInfo - Post Information</returns>
    Task<PostInfo> GetPostInfoByPostIdAsync(string PostId);

    /// <summary>
    /// This method is responsible for adding the new Post Information to the context, add identity and audit data and Save the changes in the database.
    /// </summary>
    /// <param name="Postnfo">Post Information to store.</param>
    /// <returns>PostInfo - Post Information stored.</returns>
    Task<PostInfo> AddAsync(PostInfo Postnfo);

    /// <summary>
    /// This method is responsible for updating the Post Information to the context add audit data and Save the changes in the database.
    /// </summary>
    /// <param name="PostInfo">PostInfo to update.</param>
    Task UpdateAsync(PostInfo PostInfo);
}
