using Microsoft.EntityFrameworkCore;
using PlatformService;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;
using Microsoft.Extensions.Configuration;
using PlatformService.AsyncDataServices;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

bool isProduction = true;


if (isProduction)
{

    Console.WriteLine("--> Using SQL Server DB");
    IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Production.json").Build();
    Console.WriteLine($"Connection string: {configuration["ConnectionString:PlatformsConn"]}");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(configuration["ConnectionString:PlatformsConn"]));
}
else
{
    Console.WriteLine("--> Using InMem DB");
    IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
    builder.Services.AddDbContext<AppDbContext>(opt => 
        opt.UseInMemoryDatabase("InMem")); 
}
// added from video


builder.Services.AddScoped<IPlatformRepo, PlatfromRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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


PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();
