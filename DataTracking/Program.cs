using DataTracking.Data;
using DataTracking.Helper;
using DataTracking.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//serilog
builder.Host.UseSerilog((context,services,configuration) =>
{
    //Read serilog settings from the appsettings.json
    configuration.ReadFrom.Configuration(context.Configuration);
    //Integrate DI
    configuration.ReadFrom.Services(services);
    ////Handle: enable asynchronous logging
    //configuration.WriteTo.Async(a =>
    //{
    //    a.Console();  // Wrap Console sink asynchronously.
    //    a.File("Logs/log-.txt",
    //           rollingInterval: RollingInterval.Day,
    //           retainedFileCountLimit: 30);  // Wrap File sink asynchronously.
    //});
});
//dbcontext
builder.Services.AddDbContext<DatabaseContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
//others service
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<GetUserContext>();
builder.Services.AddHttpContextAccessor();
//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//session
app.UseSession();
app.UseHttpsRedirection();

app.UseAuthorization();
//custom middleware
app.UseCheckSession();

app.MapControllers();

app.Run();
