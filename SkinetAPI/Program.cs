using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SkinetAPI.Extensions;
using SkinetAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAppServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();

IServiceProvider services  = scope.ServiceProvider;

Context context = services.GetRequiredService<Context>();

var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}

catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
