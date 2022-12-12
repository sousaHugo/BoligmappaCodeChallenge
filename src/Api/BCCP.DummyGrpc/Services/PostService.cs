using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace BCCP.DummyGrpc.Services
{
    public class PostService : Posts.PostsBase
    {
        private readonly ILogger<PostService> _logger;
        public PostService(ILogger<PostService> logger)
        {
            _logger = logger;
        }

        private PostReponse GetResponse(bool Filter = false)
        {
            var returnResult = new PostReponse();

            var firstPost = new PostModel()
            {
                Id = Guid.NewGuid().ToString(),
                Post = "Hello World, this is my post",
                Username = "john.doe"
            };
            firstPost.Tags.Add("HELLO");
            firstPost.Tags.Add("TAG");
            firstPost.Tags.Add("HISTORY");
            firstPost.Reactions.Add("LIKE");
            firstPost.Reactions.Add("SMILE");

            var secondPost = new PostModel()
            {
                Id = Guid.NewGuid().ToString(),
                Post = "Hello World, this is another post",
                Username = "john.doe"
            };
            secondPost.Tags.Add("FRENCH");
            secondPost.Tags.Add("HISTORY");
            secondPost.Reactions.Add("SMILE");

            var thirdPost = new PostModel()
            {
                Id = Guid.NewGuid().ToString(),
                Post = "Hello World, this is my post. From Peter.",
                Username = "peter.doe",
            };
            thirdPost.Tags.Add("HELLO");
            thirdPost.Tags.Add("HISTORY");

            var fourthPost = new PostModel()
            {
                Id = Guid.NewGuid().ToString(),
                Post = "Hello World, this a post from Jim Morrison.",
                Username = "jim.morrison"
            };

            returnResult.Posts.Add(firstPost);
            returnResult.Posts.Add(secondPost);
            

            if (!Filter)
            {
                returnResult.Posts.Add(thirdPost);
                returnResult.Posts.Add(fourthPost);
            }
               

            return returnResult;
        }

        public override Task<PostReponse> Get(Empty Request, ServerCallContext Context)
        {
            _logger.LogInformation("Get all Posts");

            return Task.FromResult(GetResponse());
        }

        public override Task<PostReponse> GetByTag(PostsGetByTagRequest Request, ServerCallContext Context)
        {
            _logger.LogInformation($"Get all Posts by Tag {Request.Tag}");

            return Task.FromResult(GetResponse(true));
        }

    }
}