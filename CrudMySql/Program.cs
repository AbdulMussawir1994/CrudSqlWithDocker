using CrudMySql.DataContext;
using CrudMySql.Helpers;
using CrudMySql.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "defaultdb";
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD") ?? "YourFallbackPassword";
var connectionString = $"Server={dbHost};Database={dbName};User ID=sa;Password={dbPassword};Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=True";

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = false;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

//// ✅ Database Context with SQL Server and Retry Logic
///         For Production
builder.Services.AddDbContextPool<DataDbContext>(options =>
    options.UseSqlServer(
        connectionString,
        sqlOptions =>
        {
            sqlOptions.CommandTimeout(60);
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            );
        }
    ).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);

//For Development
//builder.Services.AddDbContext<DataDbContext>(options =>
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("DefaultConnection"),
//        sqlOptions => sqlOptions.EnableRetryOnFailure()
//    )
//);

builder.Services.AddScoped<IEmployeeLayer, EmployeeLayer>();

TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton(new MapsterProfile());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
