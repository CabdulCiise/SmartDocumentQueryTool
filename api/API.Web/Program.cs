using API.Web.Extensions;
using API.Data.Extensions;
using API.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddWebServices(builder.Configuration);

builder
    .Build()
    .Configure()
    .MigrateDatabase()
    .Run();

// Need this for unit tests
public partial class Program { }