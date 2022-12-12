using BCCP.DummyAggregatorApi.Models;
using BCCP.DummyGrpc;
using Microsoft.AspNetCore.Mvc;
using static BCCP.DummyGrpc.Posts;
using static BCCP.DummyGrpc.Todos;
using static BCCP.DummyGrpc.Users;

namespace BCCP.DummyAggregatorApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserInfoController : ControllerBase
    {
        private readonly ILogger<UserInfoController> _logger;
        private readonly TodosClient _todosClient;
        private readonly PostsClient _postClient;
        private readonly UsersClient _usersClient;

        public UserInfoController(ILogger<UserInfoController> Logger, TodosClient TodosClient, PostsClient PostsClient,
            UsersClient UsersClient)
        {
            _logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
            _todosClient = TodosClient ?? throw new ArgumentNullException(nameof(TodosClient));
            _postClient = PostsClient ?? throw new ArgumentNullException(nameof(PostsClient));
            _usersClient = UsersClient ?? throw new ArgumentNullException(nameof(UsersClient));
        }

        [HttpGet("{Tag}")]
        public async Task<IEnumerable<UserInfoModel>> Get(string Tag)
        {

            _logger.LogInformation($"Processing UserInfoController - Get User Info");

            var dummyPosts = await _postClient.GetByTagAsync(new PostsGetByTagRequest() { Tag = Tag});

            var userInfoList = new List<UserInfoModel>();

            foreach(var item in dummyPosts.Posts)
            {
                var userInfo = userInfoList.FirstOrDefault(a => a.Username == item.Username);

                if(userInfo is null)
                {
                    var user = await _usersClient.GetByUsernameAsync(new DummyGrpc.UsersByUsernameRequest() { Username = item.Username });

                    var todoModel = new TodoResponse();

                    if(user is not null)
                        todoModel = await _todosClient.GetByUserIdAsync(new DummyGrpc.TodosGetByUserIdRequest() { UserId = user.Id});

                    var userInfoModel = new UserInfoModel()
                    {
                        Id = user?.Id,
                        Username = user?.Username,
                        CardType = (CardType)Enum.Parse(typeof(CardType), user?.CardType.ToString()),
                        Todos = todoModel.Todos.Select(t => new Models.TodoModel()
                        {
                            Id = t.Id,
                            Description = t.Description,
                            Title = t.Title
                        }).ToList()
                    };
                    userInfoModel.Posts.Add(new Models.PostModel()
                    {
                        Id = item.Id,
                        Post = item.Post,
                        Reactions = item.Reactions,
                        Tags = item.Tags
                    });
                    userInfoList.Add(userInfoModel);
                }
                else
                {
                    userInfo.Posts.Add(new Models.PostModel()
                    {
                        Id = item.Id,
                        Post = item.Post,
                        Reactions = item.Reactions,
                        Tags = item.Tags
                    });
                }
            }

            return userInfoList;
        }

    }
}