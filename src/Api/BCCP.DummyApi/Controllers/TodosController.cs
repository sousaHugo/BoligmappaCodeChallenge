using BCCP.DummyApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BCCP.DummyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ILogger<TodosController> _logger;
        private readonly IEnumerable<TodoModel> _todosList = new List<TodoModel>()
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
        public TodosController(ILogger<TodosController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TodoModel>> Get()
        {
            _logger.LogInformation("Get all Todos");

            return Ok(_todosList);
        }
        [HttpGet("{UserId}")]
        public ActionResult<IEnumerable<TodoModel>> Get(string UserId)
        {
            _logger.LogInformation("Get all Todos");

            return Ok(_todosList.Where(a => a.UserId == UserId));
        }
    }
}