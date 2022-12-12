using BCCP.DummyApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BCCP.DummyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IEnumerable<UserModel> _usersModel = new List<UserModel>()
            {
                new UserModel()
                {
                    Id = "45214412-319B-4838-BAF5-B6BED5BFDFA0",
                    Username = "john.doe",
                    CardType = CardType.MASTERCARD,
                    DateOfBirth = new DateTime(1990, 10, 1)
                },
                 new UserModel()
                {
                    Id = "526E656D-7460-4EAA-9934-C2FF29FAC927",
                    Username = "peter.doe",
                    CardType = CardType.VISA,
                    DateOfBirth = new DateTime(1980, 10, 1)
                },
                  new UserModel()
                {
                    Id = "D4259470-4FDB-4A48-9C0F-CDF4C013B78E",
                    Username = "Rose.Martin",
                    CardType = CardType.MASTERCARD,
                    DateOfBirth = new DateTime(1990, 8, 1)
                },
                  new UserModel()
                {
                    Id = "8B49BBB4-22B0-4F58-91AB-A32253B1DA18",
                    Username = "Jim.Morrison.Martin",
                    CardType = CardType.MASTERCARD,
                    DateOfBirth = new DateTime(1970, 8, 1)
                }
            };

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<UserModel> Get()
        {
            _logger.LogInformation("Get all Users");

            return _usersModel;
        }

        [HttpGet("{Id}")]
        public UserModel Get(string Id)
        {
            _logger.LogInformation($"Get User {Id}");

            return _usersModel.FirstOrDefault(a => a.Id == Id);
        }

        [HttpGet("ByUsername/{Username}")]
        public UserModel GetByUsername(string Username)
        {
            _logger.LogInformation($"Get User {Username}");

            return _usersModel.FirstOrDefault(a => a.Username == Username);
        }
    }
}