using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SecondApplication.AppConsole.Infrastructure;
using SecondApplication.Application.Features.GetPostsFromDummyApi;
using SecondApplication.Application.Features.GetPostsFromDummyApiGRpc;


namespace SecondApplication.AppConsole;

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
        catch(Exception ex)
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
        Console.WriteLine("Welcome to the Boligmappa Technical Test - Posts Console App");
        Console.WriteLine("");
        Console.WriteLine("Menu:");
        Console.WriteLine("1. Get Posts Data From Dummy Api");
        Console.WriteLine("2. Get Posts Data From Dummy Api (GRPC)");
        Console.WriteLine("3. Get All Post Information");
        Console.WriteLine("");
        Console.WriteLine("Select one of this options:");

        switch (Console.ReadLine())
        {
            case "1":
                await GetPostsDataFromDummyApi();
                break;
            case "2":
                await GetPostsDataFromDummyGrpc();
                break;
            case "3":
                await GetAllPostsInformation();
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
    private static async Task GetPostsDataFromDummyApi()
    {
        Console.WriteLine("GetDataFromDummyApi has started...");

        await _mediator.Send(new GetPostsFromDummyApiRequest());

        await GetAllPostsInformation();
    }
    private static async Task GetPostsDataFromDummyGrpc()
    {
        Console.WriteLine("GetPostsDataFromDummyGrpc has started...");

        var dummyData = await _mediator.Send(new GetPostsFromDummyApiGRpcRequest());

        Console.Clear();
        Console.WriteLine("GetPostsDataFromDummyGrpc Result:");
        foreach (var item in dummyData)
            Console.WriteLine(item.ToString());

    }
    private static async Task GetAllPostsInformation()
    {
        Console.WriteLine("GetAllPostsInformation has started...");

        var dummyData = await _mediator.Send(new GetPostsFromDummyApiGRpcRequest());

        Console.Clear();
        Console.WriteLine("GetAllPostsInformation Result:");
        foreach (var item in dummyData)
            Console.WriteLine(item.ToString());
    }
}