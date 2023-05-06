using AutoMapper;
using ERPServices.ReportCashFlow.API.Config;
using ERPServices.ReportCashFlow.API.Model.Context;
using ERPServices.ReportCashFlow.API.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//EF
var connection = builder.Configuration["MySqlConnection:MysqlConnectionString"];
builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 33))));


//EF FIM


//Mapper
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//Mapper FIM

builder.Services.AddScoped<ICashFlowDailyReportRepository, CashFlowDailyReportRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Report CashFlow Service" });
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// Migrate latest database changes during startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<MySQLContext>();

    dbContext.Database.Migrate();
}


app.Run();

