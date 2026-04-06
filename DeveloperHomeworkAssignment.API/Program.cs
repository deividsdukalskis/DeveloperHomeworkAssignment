using DeveloperHomeworkAssignment.API;
using DeveloperHomeworkAssignment.API.DataAccess;
using DeveloperHomeworkAssignment.API.Messaging.User;
using DeveloperHomeworkAssignment.API.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddProblemDetails(x =>
{
    x.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
    };
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<DHADbContext>();
builder.Services.AddScoped<IUserMessagePublisher, UserMessagePublisher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DHADbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapMinimalEndpoints();

app.Run();
