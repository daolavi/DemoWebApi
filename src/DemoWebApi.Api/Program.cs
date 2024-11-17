using DemoWebApi.Application.IoC;
using Microsoft.EntityFrameworkCore;
using DemoWebApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DemoWebApiContext>(opt =>
{
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("DemoWebApiConnectionString"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure
            (
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
            );
        });
});
builder.Services.AddHealthChecks();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapHealthChecks("/api/status").AllowAnonymous();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DemoWebApiContext>();
    db.Database.Migrate();
}

app.Run();

namespace DemoWebApi.Api
{
    public abstract partial class Program { }
}