using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace BCCP.DummyGrpc.Services
{
    public class UserService : Users.UsersBase
    {
        private readonly ILogger<UserService> _logger;
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public override Task<UserReponse> Get(Empty Request, ServerCallContext Context)
        {
            _logger.LogInformation("Get all Users");

            var returnResult = new UserReponse();
            returnResult.Users.Add(new UserModel()
            {
                Id = "45214412-319B-4838-BAF5-B6BED5BFDFA0",
                Username = "john.doe",
                CardType = 0
            });
            returnResult.Users.Add(new UserModel()
            {
                Id = "526E656D-7460-4EAA-9934-C2FF29FAC927",
                Username = "Peter.Doe",
                CardType = 0
            });
            returnResult.Users.Add(new UserModel()
            {
                Id = "D4259470-4FDB-4A48-9C0F-CDF4C013B78E",
                Username = "Rose.Martin",
                CardType = 1
            });
            returnResult.Users.Add(new UserModel()
            {
                Id = "8B49BBB4-22B0-4F58-91AB-A32253B1DA18",
                Username = "Jim.Morrison.Martin",
                CardType = 1
            });

            return Task.FromResult(returnResult);
        }

        public override Task<UserModel> GetByUsername(UsersByUsernameRequest Request, ServerCallContext Context)
        {
            _logger.LogInformation($"Get User {Request.Username}");

            var returnResult = new List<UserModel>();
            returnResult.Add(new UserModel()
            {
                Id = "45214412-319B-4838-BAF5-B6BED5BFDFA0",
                Username = "john.doe",
                CardType = 0
            });
            returnResult.Add(new UserModel()
            {
                Id = "526E656D-7460-4EAA-9934-C2FF29FAC927",
                Username = "Peter.Doe",
                CardType = 0
            });
            returnResult.Add(new UserModel()
            {
                Id = "D4259470-4FDB-4A48-9C0F-CDF4C013B78E",
                Username = "Rose.Martin",
                CardType = 1
            });
            returnResult.Add(new UserModel()
            {
                Id = "8B49BBB4-22B0-4F58-91AB-A32253B1DA18",
                Username = "Jim.Morrison.Martin",
                CardType = 1
            });

            return Task.FromResult(returnResult.FirstOrDefault(a => a.Username == Request.Username));
        }
    }
}