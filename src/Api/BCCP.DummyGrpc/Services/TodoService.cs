using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace BCCP.DummyGrpc.Services
{
    public class TodoService : Todos.TodosBase
    {
        private readonly ILogger<TodoService> _logger;
        private readonly List<TodoModel> _todosModel = new List<TodoModel>()
        {
            new TodoModel()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "My Todo is Aamazing",
                Description = "This is a description of my amazing Todo",
                UserId = "45214412-319B-4838-BAF5-B6BED5BFDFA0"
            },
            new TodoModel()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Another",
                Description = "This is a description of another amazing Todo",
                UserId = "45214412-319B-4838-BAF5-B6BED5BFDFA0"
            },
            new TodoModel()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "My Third Todo is Aamazing",
                Description = "This is a description of my third amazing Todo",
                UserId = "45214412-319B-4838-BAF5-B6BED5BFDFA0"
            },
            new TodoModel()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Another Todo",
                Description = "This is a description of another Todo",
                UserId = "526E656D-7460-4EAA-9934-C2FF29FAC927"
            }

        };

        public TodoService(ILogger<TodoService> logger)
        {
            _logger = logger;
        }

        public override Task<TodoResponse> Get(Empty Request, ServerCallContext Context)
        {
            _logger.LogInformation("Get all Todos");

            var returnResult = new TodoResponse();
            foreach(var item in _todosModel)
                returnResult.Todos.Add(item);


            return Task.FromResult(returnResult);
        }

        public override Task<TodoResponse> GetByUserId(TodosGetByUserIdRequest Request, ServerCallContext Context)
        {
            _logger.LogInformation($"Get all Todos of User Id: {Request.UserId}");

            var returnResult = new TodoResponse();
            foreach (var item in _todosModel.Where(a => a.UserId == Request.UserId))
                returnResult.Todos.Add(item);


            return Task.FromResult(returnResult);
        }
    }
}