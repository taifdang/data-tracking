using DataTracking.Data;
using DataTracking.Helper;
using DataTracking.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//serilog
builder.Host.UseSerilog((context, logger) =>
{
    logger.ReadFrom.Configuration(context.Configuration);
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
