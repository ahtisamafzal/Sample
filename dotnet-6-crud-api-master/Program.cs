using System.Text.Json.Serialization;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add services to DI container
{
    var services = builder.Services;
    var env = builder.Environment;
 
    services.AddDbContext<DataContext>();
    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // ignore omitted parameters on models to enable optional params (e.g. User update)
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // configure DI for application services
    services.AddScoped<IUserService<User>, UserService<User>>();
    
}

var app = builder.Build();

// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.UseSwagger();
    app.MapGet("/", () => ".NET 6.0 - Connect to MySQL Database with Entity Framework Core");
    app.UseSwaggerUI();

    app.MapControllers();
}

app.Run("http://localhost:4000");