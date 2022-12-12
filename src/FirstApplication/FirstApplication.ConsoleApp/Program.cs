using FirstApplication.AppConsole.Infrastructure;
using FirstApplication.Application.Features.GetAllUserInformation;
using FirstApplication.Application.Features.GetFromDummyApi;
using FirstApplication.Application.Features.GetFromDummyApiAggregator;
using FirstApplication.Application.Features.GetFromDummyApiExtra;
using FirstApplication.Application.Features.GetFromDummyApiGrpc;
using FirstApplication.Application.Features.GetPostsUsersMasterCard;
using FirstApplication.Application.Features.GetTodosUsersMoreTwoPosts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FirstApplication.AppConsole;

public class Program
{
    private static readonly ILogger<Program> _logger;
    private static readonly IMediator _mediator;
    
    static Program()
    {
        var builder = ProgramHostBuilder.CreateHostBuilder().Build();
        _logger = builder.Services.GetService<ILogger<Program>>();
        _mediator = builder.Services.GetService<IMediator>();

        _logger.LogInformation("The Program successfully initialized.");
    }
    static async Task Main()
    {
        _logger.LogInformation("The Program successfully started.");

        try
        {
            await Process();
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error as occurred trying to complete the selected action. Exception: {ex.Message}.");
            Console.WriteLine("It's not possible to complete the required action. For more information consult the log files. Press any key to continue.");
            Console.ReadLine();
            await Process();
        }
    }  

    static async Task Process()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the Boligmappa Technical Test - Users Console App");
        Console.WriteLine("");
        Console.WriteLine("Menu:");
        Console.WriteLine("1. Get Data From Dummy Api");
        Console.WriteLine("2. Get Data From Dummy Api (Extra)");
        Console.WriteLine("3. Get Data From Dummy Api (GRPC)");
        Console.WriteLine("4. Get Data From Dummy Api (Aggregator)");
        Console.WriteLine("5. Todos From Users with More than 2 Posts");
        Console.WriteLine("6. Posts From Users with MasterCard");
        Console.WriteLine("");
        Console.WriteLine("Select one of this options:");

        switch (Console.ReadLine())
        {
            case "1":
                await GetDataFromDummyApi();
                break;
            case "2":
                await GetDataFromDummyApi(2);
                break;
            case "3":
                await GetDataFromDummyApi(3);
                break;
            case "4":
                await GetDataFromDummyApi(4);
                break;
            case "5":
                await GetTodosFromUsers();
                break;
            case "6":
                await GetPostsFromUsers();
                break;
                default:
                    Console.WriteLine($"You should select one of those options. Press any key to continue.");
                    Console.ReadLine();
                    await Process();
                break;
        }

        Console.WriteLine("Press any key to return to the menu.");
        Console.ReadLine();
        await Process();
    }
    private static async Task GetDataFromDummyApi(int Option = 1)
    {
        Console.WriteLine("GetDataFromDummyApi has started...");

        if(Option == 1)
            _ = await _mediator.Send(new GetFromDummyApiRequest());
        else if (Option == 2)
            _ = await _mediator.Send(new GetFromDummyApiExtraRequest());
        else if (Option == 4)
            _ = await _mediator.Send(new GetFromDummyApiAggregatorRequest());
        else if (Option == 3)
            _ = await _mediator.Send(new GetFromDummyApiGrpcRequest());

        var dummyData = await _mediator.Send(new GetAllUserInformationRequest());

        Console.Clear();
        Console.WriteLine("GetDataFromDummyApi Result:");
        foreach (var item in dummyData)
            Console.WriteLine(item.ToString());
    }
    private static async Task GetTodosFromUsers()
    {
        Console.WriteLine("GetTodosFromUsers has started...");

        var todosResult = await _mediator.Send(new GetTodosUsersMoreTwoPostsRequest());

        Console.Clear();
        Console.WriteLine("GetTodosFromUsers Result:");

        foreach (var todo in todosResult)
            Console.WriteLine(todo.ToString());
    }
    private static async Task GetPostsFromUsers()
    {
        Console.WriteLine("GetPostsFromUsers has started...");

        var postsResult = await _mediator.Send(new GetPostsUsersMasterCardRequest());

        Console.Clear();
        Console.WriteLine("GetPostsFromUsers Result:");

        foreach (var post in postsResult)
            Console.WriteLine(post.ToString());
    }
}