using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using SecondApplication.Application.Contracts.Repositories;
using SecondApplication.Application.Dtos;
using SecondApplication.Application.Features.GetAllPostInfo;
using SecondApplication.Application.Mapping;
using SecondApplication.Domain.Entities;
using Shouldly;

namespace SecondApplication.Tests.Features
{
    public class GetAllPostInfoTest
    {
        private readonly GetAllPostInfoRequestHandler _handler;

        private readonly Mock<IPostInfoRepository> _postInfoRepositoryMock;
        private readonly Mock<ILogger<GetAllPostInfoRequestHandler>> _loggerMock;
        private readonly IMapper _mapperMock;
        
        private List<PostInfo> _dbPosts = new();

        public GetAllPostInfoTest()
        {
            _postInfoRepositoryMock = new Mock<IPostInfoRepository>();
            _loggerMock = new Mock<ILogger<GetAllPostInfoRequestHandler>>();
            _mapperMock = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new ApplicationMappingProfile())));

            _postInfoRepositoryMock.Setup(m => m.GetAllAsync()).ReturnsAsync(() => _dbPosts);

            _handler = new GetAllPostInfoRequestHandler(_postInfoRepositoryMock.Object, _loggerMock.Object, _mapperMock);
        }

        [Fact]
        public void GetAllPostInfoRequestHandler_NoResultsSuccess()
        {
            var result = _handler.Handle(new GetAllPostInfoRequest() { }, default).Result;

            result.ShouldBeAssignableTo<IEnumerable<PostInfoDto>>();
            result.ShouldBeEmpty();

        }

        [Fact]
        public void GetAllPostInfoRequestHandler_OneResultSuccess()
        {
            _dbPosts.Add(new PostInfo
            {
                CreatedDate = DateTime.Now,
                HasFictonTag = true,
                HasFrenchTag = true,
                HasMoreThanTwoReactions = true,
                Id = Guid.NewGuid().ToString(),
                ModifiedDate = DateTime.Now,
                PostId = Guid.NewGuid().ToString(),
                Username = "john.doe"
            });

            var result = _handler.Handle(new GetAllPostInfoRequest() { }, default).Result;

            result.ShouldBeAssignableTo<IEnumerable<PostInfoDto>>();
            result.ShouldHaveSingleItem();

        }

        [Fact]
        public void GetAllPostInfoRequestHandler_OneResultEqualInfoSuccess()
        {
            var id = Guid.NewGuid().ToString();
            var postId = Guid.NewGuid().ToString();

            var userInfo = new PostInfo
            {
                CreatedDate = DateTime.Today,
                HasFictonTag = true,
                HasFrenchTag = true,
                HasMoreThanTwoReactions = true,
                Id = id,
                ModifiedDate = DateTime.Today,
                PostId = postId,
                Username = "john.doe"
            };

            var expectedUserInfo = new PostInfo
            {
                CreatedDate = DateTime.Today,
                HasFictonTag = true,
                HasFrenchTag = true,
                HasMoreThanTwoReactions = true,
                Id = id,
                ModifiedDate = DateTime.Today,
                PostId = postId,
                Username = "john.doe"
            };

            _dbPosts.Add(userInfo);

            var result = _handler.Handle(new GetAllPostInfoRequest() { }, default).Result;

            result.ShouldBeAssignableTo<IEnumerable<PostInfoDto>>();
            result.ShouldHaveSingleItem();

            var firstResult = result.FirstOrDefault();
            firstResult.ShouldNotBeNull();
            
            firstResult.CreatedDate.ShouldBeEquivalentTo(expectedUserInfo.CreatedDate);
            firstResult.HasFictonTag.ShouldBeEquivalentTo(expectedUserInfo.HasFictonTag);
            firstResult.HasFrenchTag.ShouldBeEquivalentTo(expectedUserInfo.HasFrenchTag);
            firstResult.HasMoreThanTwoReactions.ShouldBeEquivalentTo(expectedUserInfo.HasMoreThanTwoReactions);
            firstResult.Id.ShouldBeEquivalentTo(expectedUserInfo.Id);
            firstResult.ModifiedDate.ShouldBeEquivalentTo(expectedUserInfo.ModifiedDate);
            firstResult.PostId.ShouldBeEquivalentTo(expectedUserInfo.PostId);
            firstResult.Username.ShouldBeEquivalentTo(expectedUserInfo.Username);

        }
    }
}