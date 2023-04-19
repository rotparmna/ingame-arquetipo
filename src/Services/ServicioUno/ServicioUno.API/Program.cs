using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using ServicioUno.API.Infrastructure;
using ServicioUno.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AddCustomDbContext(builder.Services, builder.Configuration);
AddHttpClient(builder.Services, builder.Configuration);
builder.Host.UseSerilog(CreateSerilogLogger(builder.Configuration));

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

Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    return new LoggerConfiguration()
        .WriteTo.Console().ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
}

void AddCustomDbContext(IServiceCollection services, IConfiguration configuration)
{
    services.AddEntityFrameworkNpgsql()
        .AddDbContext<ServicioUnoContext>(options => {
            options.UseNpgsql(configuration["ConnectionString"],
                                    npgsqlOptionsAction: sqlOptions => 
                                    {
                                        sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, 
                                            maxRetryDelay: TimeSpan.FromSeconds(30), 
                                            errorCodesToAdd: null);
                                    });
        });
}

void AddHttpClient(IServiceCollection services, IConfiguration configuration)
{
    services.AddHttpClient<IServicioDosService, ServicioDosService>(client =>
    {
        client.BaseAddress = new Uri(configuration["BaseUrl"]);
    })
        .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(GetRetryPolicy());
}

IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

public partial class Program
{

    public static string Namespace = typeof(Program).Assembly.GetName().Name;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}