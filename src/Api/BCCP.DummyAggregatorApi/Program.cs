using static BCCP.DummyGrpc.Posts;
using static BCCP.DummyGrpc.Todos;
using static BCCP.DummyGrpc.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpcClient<PostsClient>(o =>
{
    o.Address = new Uri(builder.Configuration["ApiUrls:DummyGrpcUrl"]);
});


builder.Services.AddGrpcClient<TodosClient>(o =>
{
    o.Address = new Uri(builder.Configuration["ApiUrls:DummyGrpcUrl"]);
});


builder.Services.AddGrpcClient<UsersClient>(o =>
{
    o.Address = new Uri(builder.Configuration["ApiUrls:DummyGrpcUrl"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
