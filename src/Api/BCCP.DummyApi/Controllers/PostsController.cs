using BCCP.DummyApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BCCP.DummyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IEnumerable<PostModel> _postsList = new List<PostModel>()
           {
               new PostModel()
               {
                   Id = "c88f3229-d0f5-4ddd-abed-5756ce5e92c8",
                   Post = "Hello World, this is my post",
                   Username = "john.doe",
                   Tags = new List<string>(){ "HELLO", "TAG", "HISTORY" },
                   Reactions = new List<string>() { "SMILE" }
               },
               new PostModel()
               {
                   Id = "5ceace5d-5d68-4987-8a9a-c9ad20d735cd",
                   Post = "Hello World, this is another post",
                   Username = "john.doe",
                   Tags = new List<string>(){ "FRENCH", "HISTORY" },
                   Reactions = new List<string>() { "LIKE" }
               },
               new PostModel()
               {
                   Id = "8f2552ab-a763-45ed-a6d2-ca626156e13d",
                   Post = "Hello World, this is my post. From Peter.",
                   Username = "john.doe",
                   Tags = new List<string>(){ "HELLO", "HISTORY" },
                   Reactions = new List<string>() { "SMILE", "CLAP" }
               },
               new PostModel()
               {
                   Id = "b8a97054-b905-43b7-9792-f940ca74146e",
                   Post = "Hello World, this a post from Jim Morrison.",
                   Username = "jim.morrison",
               }
           };
        public PostsController(ILogger<PostsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PostModel>> Get()
        {
            _logger.LogInformation("Get all Posts");

            return Ok(_postsList);
        }
        [HttpGet("ByUsername/{Username}")]
        public ActionResult<IEnumerable<PostModel>> Get(string Username)
        {
            _logger.LogInformation($"Get all Posts by {Username}");

            return Ok(_postsList.Where(a => a.Username == Username));
        }
        [HttpGet("ByTag/{Tag}")]
        public ActionResult<IEnumerable<PostModel>> GetByTag(string Tag)
        {
            _logger.LogInformation($"Get all Posts by {Tag}");

            return Ok(_postsList.Where(a => a.Tags.Contains(Tag)));
        }
    }
}