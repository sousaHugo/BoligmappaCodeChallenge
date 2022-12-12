using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using SecondApplication.Application.Contracts.Repositories;
using SecondApplication.Application.Contracts.Services;
using SecondApplication.Application.Dtos;
using SecondApplication.Application.Features.GetPostsFromDummyApi;
using SecondApplication.Application.Mapping;
using SecondApplication.Domain.Entities;
using Shouldly;

namespace SecondApplication.Tests.Features;

public class GetPostsFromDummyApiTest
{
    private readonly GetPostsFromDummyApiRequestHandler _handler;

    private readonly Mock<IPostInfoRepository> _postInfoRepositoryMock;
    private readonly Mock<IPostService> _postServiceMock;
    private readonly Mock<ILogger<GetPostsFromDummyApiRequestHandler>> _loggerMock;
    private readonly IMapper _mapperMock;

    private readonly List<PostInfo> _dbPosts = new();
    private readonly List<PostDto> _dtoPosts = new();

    private string _newPostId;

    public GetPostsFromDummyApiTest()
    {
        _postInfoRepositoryMock = new Mock<IPostInfoRepository>();
        _postServiceMock = new Mock<IPostService>();
        _loggerMock = new Mock<ILogger<GetPostsFromDummyApiRequestHandler>>();
        _mapperMock = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new ApplicationMappingProfile())));

        _postInfoRepositoryMock.Setup(m => m.GetPostInfoByPostIdAsync(It.IsAny<string>())).ReturnsAsync((string PostId) =>
        {
            return _dbPosts.SingleOrDefault(a => a.PostId == PostId);
        });
        _postInfoRepositoryMock.Setup(m => m.AddAsync(It.IsAny<PostInfo>())).ReturnsAsync((PostInfo Post) => {

            Post.Id = _newPostId;
            _dbPosts.Add(Post);

            return Post;
        });
        _postInfoRepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<PostInfo>())).Callback((PostInfo Post) => {

            var post = _dbPosts.Single(a => a.PostId == Post.PostId);
            var indexOf = _dbPosts.IndexOf(post);
            _dbPosts[indexOf] = Post;

        });
        _postServiceMock.Setup(p => p.GetAllPostsAsync<IEnumerable<PostDto>>()).ReturnsAsync(() => _dtoPosts);

        _handler = new GetPostsFromDummyApiRequestHandler(_postServiceMock.Object, _postInfoRepositoryMock.Object, _loggerMock.Object, _mapperMock);
    }
    [Fact]
    public void GetPostsFromDummyApiRequestHandler_AddPostSuccess()
    {
        var post = new PostDto()
        {
            Id = "1234",
            Post = "Teste",
            Reactions = new List<string>() { "smile" },
            Username = "john.doe"
        };

        _dtoPosts.Add(post);

        _newPostId = "1234";

        _ = _handler.Handle(new GetPostsFromDummyApiRequest() { }, default).Result;

        _dbPosts.ShouldNotBeEmpty();
        _dbPosts.ShouldNotBeNull();
        _dtoPosts.Count.ShouldBe(_dbPosts.Count);

        var newPost = _dbPosts.FirstOrDefault();
        newPost.ShouldNotBeNull();

        newPost.Username.ShouldBe(post.Username);
        newPost.HasFictonTag.ShouldBeFalse();
        newPost.HasFrenchTag.ShouldBeFalse();
        newPost.HasMoreThanTwoReactions.ShouldBeFalse();
        newPost.Id.ShouldBe(post.Id);

    }

    [Fact]
    public void GetPostsFromDummyApiRequestHandler_AddPostFailValidation()
    {
        var post = new PostDto()
        {
            Id = "1234",
            Post = "Teste",
            Reactions = new List<string>() { "smile" }
        };

        _dtoPosts.Add(post);

        _newPostId = "1234";

        _ = _handler.Handle(new GetPostsFromDummyApiRequest() { }, default).Result;

        _dbPosts.ShouldBeEmpty();
    }
    [Fact]
    public void GetPostsFromDummyApiRequestHandler_UpdatePostSuccess()
    {
        var post = new PostInfo()
        {
            Id = "1234",
            Username = "john.doe",
            CreatedDate = DateTime.Now,
            HasFictonTag = false,
            HasFrenchTag = true,
            HasMoreThanTwoReactions = false,
            ModifiedDate = DateTime.Now,
            PostId = "123455"
        };

        _dbPosts.Add(post);

        _dtoPosts.Add(new PostDto()
        {
            Id = "123455",
            Post = "Test 1",
            Tags = new List<string>() { "FICTION", "FRENCH", "TEST" },
            Username = "john.doe"
        });

        _dtoPosts.Add(new PostDto()
        {
            Id = "1234556",
            Post = "Test 2",
            Username = "john.doe"
        });


        _ = _handler.Handle(new GetPostsFromDummyApiRequest() { }, default).Result;

        _dbPosts.Count.ShouldBe(_dtoPosts.Count);

        var updated = _dbPosts.SingleOrDefault(a => a.PostId == post.PostId);
        updated.ShouldNotBeNull();
        updated.HasFictonTag.ShouldBeTrue();
        updated.HasFrenchTag.ShouldBeTrue();
        updated.HasMoreThanTwoReactions.ShouldBeTrue();

        var newPost = _dbPosts.SingleOrDefault(a => a.PostId == "1234556");
        newPost.ShouldNotBeNull();
        newPost.HasFictonTag.ShouldBeFalse();
        newPost.HasFrenchTag.ShouldBeFalse();
        newPost.HasMoreThanTwoReactions.ShouldBeFalse();
    }
}
