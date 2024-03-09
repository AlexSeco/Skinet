using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Infrastructure.Identity.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkinetAPI.Extensions;
using SkinetAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAppServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseSwaggerDocumentation();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();

IServiceProvider services = scope.ServiceProvider;

Context context = services.GetRequiredService<Context>();
AppIdentityDbContext identityContext = services.GetRequiredService<AppIdentityDbContext>();
UserManager<AppUser> userManager = services.GetRequiredService<UserManager<AppUser>>();

var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    await context.Database.MigrateAsync();
    await identityContext.Database.MigrateAsync();
    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.DeliveryMethods ON");
    await StoreContextSeed.SeedAsync(context);
    await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.DeliveryMethods OFF");

    await AppIdentityDbContextSeed.SeedUserAsync(userManager);
}

catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during migration");
}



app.Run();
