using AutoMapper;
using FirstApplication.Application.Contracts.Repositories;
using FirstApplication.Application.Contracts.Services;
using FirstApplication.Application.Dtos;
using FirstApplication.Application.Features.GetFromDummyApiAggregator;
using FirstApplication.Application.Mapping;
using FirstApplication.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace FirstApplication.Tests.Features;

public class GetFromDummyApiAggregatorTest
{
    private readonly GetFromDummyApiAggregatorRequestHandler _handler;
   
    private readonly Mock<IUserInfoRepository> _userInfoRepositoryMock;
    private readonly Mock<IUserInfoService> _userinfoServiceMock;
    private readonly Mock<ILogger<GetFromDummyApiAggregatorRequestHandler>> _loggerMock;
    private readonly IMapper _mapperMock;

    private readonly List<UserInfo> _dbUsers = new();
    private readonly List<UserInfoDto> _dtoUsers = new();

    private string _newUserInfoId;

    public GetFromDummyApiAggregatorTest()
    {
        _userInfoRepositoryMock = new Mock<IUserInfoRepository>();
        _userinfoServiceMock = new Mock<IUserInfoService>();

        _loggerMock = new Mock<ILogger<GetFromDummyApiAggregatorRequestHandler>>();

        _userInfoRepositoryMock.Setup(m => m.GetByUserIdAsync(It.IsAny<string>())).ReturnsAsync((string UserId) => {

            return _dbUsers.SingleOrDefault(a => a.UserId == UserId);
        });
        _userInfoRepositoryMock.Setup(m => m.AddAsync(It.IsAny<UserInfo>())).ReturnsAsync((UserInfo User) => {

            User.Id = _newUserInfoId;
            _dbUsers.Add(User);

            return User;
        });
        _userInfoRepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<UserInfo>())).Callback((UserInfo User) => {

            var post = _dbUsers.Single(a => a.UserId == User.UserId);
            var indexOf = _dbUsers.IndexOf(post);
            _dbUsers[indexOf] = User;

        });
        _userinfoServiceMock.Setup(m => m.GetAllUserInfoAsync<IEnumerable<UserInfoDto>>(It.IsAny<string>()))
            .ReturnsAsync(() => _dtoUsers);

        _mapperMock = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new ApplicationMappingProfile())));

        _handler = new GetFromDummyApiAggregatorRequestHandler(_userinfoServiceMock.Object,
            _userInfoRepositoryMock.Object, _loggerMock.Object, _mapperMock);
    }

    [Fact]
    public void GetFromDummyApiExtraRequestHandler_AddPostSuccess()
    {
        var userInfoDto = new UserInfoDto()
        {
            CreatedDate = DateTime.Today,
            UserId = "1223",
            ModifiedDate = DateTime.Today,
            NumberOfPosts = 1,
            NumberOfTodos = 1,
            UseMasterCard = true,
            Username = "user.name"
        };

        _dtoUsers.Add(userInfoDto);

        _newUserInfoId = "UID1";

        _ = _handler.Handle(new GetFromDummyApiAggregatorRequest() { }, default).Result;

        _dbUsers.ShouldNotBeEmpty();
        _dbUsers.ShouldNotBeNull();
        _dtoUsers.Count.ShouldBe(_dbUsers.Count);

        var newUser = _dbUsers.FirstOrDefault();
        newUser.ShouldNotBeNull();

        newUser.Username.ShouldBe(userInfoDto.Username);
        newUser.UseMasterCard.ShouldBeTrue();
        newUser.Id.ShouldBe(_newUserInfoId);
        newUser.CreatedDate.ShouldBe(userInfoDto.CreatedDate);
        newUser.ModifiedDate.ShouldBe(userInfoDto.ModifiedDate);
        newUser.NumberOfTodos.ShouldBe(userInfoDto.NumberOfTodos);
        newUser.NumberOfPosts.ShouldBe(userInfoDto.NumberOfPosts);

    }

    [Fact]
    public void GetFromDummyApiExtraRequestHandler_AddPostFailValidation()
    {
        var userInfoDto = new UserInfoDto()
        {
            CreatedDate = DateTime.Today,
            ModifiedDate = DateTime.Today,
            NumberOfPosts = 1,
            NumberOfTodos = 1,
            UseMasterCard = true,
            Username = "user.name"
        };

        _dtoUsers.Add(userInfoDto);

        _newUserInfoId = "UID1";

        _ = _handler.Handle(new GetFromDummyApiAggregatorRequest() { }, default).Result;

        _dbUsers.ShouldBeEmpty();
    }
    [Fact]
    public void GetFromDummyApiExtraRequestHandler_UpdatePostSuccess()
    {
        var userInfo = new UserInfo()
        {
            Id = "1234",
            Username = "john.doe",
            CreatedDate = DateTime.Today,
            ModifiedDate = DateTime.Today,
            UserId = "U1",
            UseMasterCard = true,
            NumberOfPosts = 1,
            NumberOfTodos = 1
        };

        _dbUsers.Add(userInfo);

        var userInfoDto = new UserInfoDto()
        {
            Username = "john.doe",
            CreatedDate = DateTime.Today,
            ModifiedDate = DateTime.Today,
            UserId = "U1",
            UseMasterCard = false,
            NumberOfPosts = 0,
            NumberOfTodos = 0
        };

        _dtoUsers.Add(userInfoDto);

        _ = _handler.Handle(new GetFromDummyApiAggregatorRequest() { }, default).Result;


        var updated = _dbUsers.SingleOrDefault(a => a.UserId == userInfo.UserId);
        updated.ShouldNotBeNull();
        updated.UseMasterCard.ShouldBeFalse();
        updated.NumberOfTodos.ShouldBe(0);
        updated.NumberOfPosts.ShouldBe(0);
    }
}
