using AutoMapper;
using FirstApplication.Application.Contracts.Repositories;
using FirstApplication.Application.Dtos;
using FirstApplication.Application.Features.GetAllUserInformation;
using FirstApplication.Application.Mapping;
using FirstApplication.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace FirstApplication.Tests.Features
{
    public class GetAllUserInformationTest
    {
        private readonly GetAllUserInformationRequestHandler _handler;

        private readonly Mock<IUserInfoRepository> _userInfoRepositoryMock;
        private readonly Mock<ILogger<GetAllUserInformationRequestHandler>> _loggerMock;
        private readonly IMapper _mapperMock;
        
        private readonly List<UserInfo> _dbUsers = new();

        public GetAllUserInformationTest()
        {
            _userInfoRepositoryMock = new Mock<IUserInfoRepository>();
            _loggerMock = new Mock<ILogger<GetAllUserInformationRequestHandler>>();
            _mapperMock = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new ApplicationMappingProfile())));


            _userInfoRepositoryMock.Setup(m => m.GetAllAsync()).ReturnsAsync(() => _dbUsers);

            _handler = new GetAllUserInformationRequestHandler(_userInfoRepositoryMock.Object, _mapperMock, _loggerMock.Object);
        }

        [Fact]
        public void GetAllUserInformationRequestHandler_NoResultsSuccess()
        {
            var result = _handler.Handle(new GetAllUserInformationRequest() { }, default).Result;

            result.ShouldBeAssignableTo<IEnumerable<GetAllUserInfoDto>>();
            result.ShouldBeEmpty();
        }
        [Fact]
        public void GetAllUserInformationRequestHandler_OneResultSuccess()
        {
            _dbUsers.Add(new UserInfo
            {
                CreatedDate = DateTime.Now,
                Id = Guid.NewGuid().ToString(),
                ModifiedDate = DateTime.Now,
                Username = "john.doe",
                NumberOfPosts = 1,
                NumberOfTodos = 1,
                UseMasterCard = true,
                UserId = "1234"
            });

            var result = _handler.Handle(new GetAllUserInformationRequest() { }, default).Result;

            result.ShouldBeAssignableTo<IEnumerable<GetAllUserInfoDto>>();
            result.ShouldHaveSingleItem();
        }
        [Fact]
        public void GetAllPostInfoRequestHandler_OneResultEqualInfoSuccess()
        {
            var id = Guid.NewGuid().ToString();

            var userInfo = new UserInfo
            {
                CreatedDate = DateTime.Today,
                Id = id,
                ModifiedDate = DateTime.Today,
                Username = "john.doe",
                NumberOfPosts = 1,
                NumberOfTodos = 1,
                UseMasterCard = true,
                UserId = "1234"
            };

            var expectedUserInfo = new UserInfo
            {
                CreatedDate = DateTime.Today,
                Id = id,
                ModifiedDate = DateTime.Today,
                Username = "john.doe",
                NumberOfPosts = 1,
                NumberOfTodos = 1,
                UseMasterCard = true,
                UserId = "1234"
            };

            _dbUsers.Add(userInfo);

            var result = _handler.Handle(new GetAllUserInformationRequest() { }, default).Result;

            result.ShouldBeAssignableTo<IEnumerable<GetAllUserInfoDto>>();
            result.ShouldHaveSingleItem();

            var firstResult = result.FirstOrDefault();
            firstResult.ShouldNotBeNull();

            firstResult.CreatedDate.ShouldBeEquivalentTo(expectedUserInfo.CreatedDate);
            firstResult.NumberOfPosts.ShouldBeEquivalentTo(expectedUserInfo.NumberOfPosts);
            firstResult.NumberOfTodos.ShouldBeEquivalentTo(expectedUserInfo.NumberOfTodos);
            firstResult.UseMasterCard.ShouldBeEquivalentTo(expectedUserInfo.UseMasterCard);
            firstResult.Id.ShouldBeEquivalentTo(expectedUserInfo.Id);
            firstResult.ModifiedDate.ShouldBeEquivalentTo(expectedUserInfo.ModifiedDate);
            firstResult.Username.ShouldBeEquivalentTo(expectedUserInfo.Username);
            firstResult.UserId.ShouldBeEquivalentTo(expectedUserInfo.UserId);

        }
    }
}