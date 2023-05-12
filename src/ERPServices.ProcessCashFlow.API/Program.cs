using ERPServices.ProcessCashFlow.API.Model.Context;
using ERPServices.ProcessCashFlow.API.Repository;
using ERPServices.Report.API.RabbitMQConsumer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//EF
var connection = builder.Configuration["MySqlConnection:MysqlConnectionString"];
builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 33))));

var dbBuilder = new DbContextOptionsBuilder<MySQLContext>();
dbBuilder.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 33)));
//EF FIM

builder.Services.AddSingleton(new CashFlowDailyProcessRepository(dbBuilder.Options));

builder.Services.AddHostedService<RabbitMQMessageConsumer>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Process CashFlow Service" });
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "DevelopmentIDE")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
