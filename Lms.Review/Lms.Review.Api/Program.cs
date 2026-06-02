using Microsoft.EntityFrameworkCore;
using Review.Application.Interfaces;
using Review.Application.Services;
using Review.Infrastructure.Data;
using Review.Infrastructure.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ReviewDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Reviews API";

        document.Info.Version = "v1";

        document.Info.Description =
            "API for handling course reviews, comments and review management.";

        return Task.CompletedTask;
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:3000",
                "https://lms-shiko.vercel.app")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.MapOpenApi();

// Scalar UI
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("Reviews API")
        .WithTheme(ScalarTheme.BluePlanet)
        .WithDefaultHttpClient(
            ScalarTarget.CSharp,
            ScalarClient.HttpClient
        );
});

app.MapControllers();

app.Run();