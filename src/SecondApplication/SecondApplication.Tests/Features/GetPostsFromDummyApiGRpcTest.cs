using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using SecondApplication.Application.Contracts.Services.Grpc;
using SecondApplication.Application.Dtos;
using SecondApplication.Application.Features.GetPostsFromDummyApiGRpc;
using SecondApplication.Application.Mapping;
using SecondApplication.Domain.Models;
using Shouldly;

namespace SecondApplication.Tests.Features
{
    public class GetPostsFromDummyApiGRpcTest
    {
        private readonly GetPostsFromDummyApiGRpcRequestHandler _handler;

        private readonly Mock<IPostGrpcService> _postGrpcServiceMocker;
        private readonly Mock<ILogger<GetPostsFromDummyApiGRpcRequestHandler>> _loggerMock;
        private readonly IMapper _mapperMock;

        private readonly List<PostGrpcModel> _modelPostsGrpc = new();

        public GetPostsFromDummyApiGRpcTest()
        {
            _postGrpcServiceMocker = new Mock<IPostGrpcService>();
            _loggerMock = new Mock<ILogger<GetPostsFromDummyApiGRpcRequestHandler>>();
            _mapperMock = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new ApplicationMappingProfile())));

            _postGrpcServiceMocker.Setup(p => p.GetAllPostsAsync(default))
                .ReturnsAsync(() => _modelPostsGrpc);

            _handler = new GetPostsFromDummyApiGRpcRequestHandler(_postGrpcServiceMocker.Object, _mapperMock, _loggerMock.Object);
        }

        [Fact]
        public void GetPostsFromDummyApiGRpcRequestHandler_NoResultsSuccess()
        {
            var result = _handler.Handle(new GetPostsFromDummyApiGRpcRequest() { }, default).Result;

            result.ShouldBeAssignableTo<IEnumerable<PostDto>>();
            result.ShouldBeEmpty();

        }

        [Fact]
        public void GetPostsFromDummyApiGRpcRequestHandler_OneResultSuccess()
        {
            _modelPostsGrpc.Add(new PostGrpcModel
            {
                Id = Guid.NewGuid().ToString(),
                Username = "john.doe",
                Post = "New Post",
                Reactions = new List<string>() { "Smile"}
            });

            var result = _handler.Handle(new GetPostsFromDummyApiGRpcRequest() { }, default).Result;

            result.ShouldBeAssignableTo<IEnumerable<PostDto>>();
            result.ShouldHaveSingleItem();

        }

        [Fact]
        public void GetPostsFromDummyApiGRpcRequestHandler_OneResultEqualInfoSuccess()
        {
            var id = Guid.NewGuid().ToString();

            var userInfo = new PostGrpcModel
            {
                Id = id,
                Username = "john.doe",
                Post = "New Post",
                Reactions = new List<string>() { "Smile" }
            };


            _modelPostsGrpc.Add(userInfo);

            var result = _handler.Handle(new GetPostsFromDummyApiGRpcRequest() { }, default).Result;

            result.ShouldBeAssignableTo<IEnumerable<PostDto>>();
            result.ShouldHaveSingleItem();

            var firstResult = result.FirstOrDefault();
            firstResult.ShouldNotBeNull();

            firstResult.Id.ShouldBe(userInfo.Id);
            firstResult.Username.ShouldBe(userInfo.Username);
            firstResult.Post.ShouldBe(userInfo.Post);
            firstResult.Reactions.ShouldBe(userInfo.Reactions);
            firstResult.Tags.ShouldBeEmpty();

        }
    }
}