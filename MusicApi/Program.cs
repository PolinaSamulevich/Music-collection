using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Models;
using MusicApi.Repositories;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MusicdbContext>(options =>
    options.UseSqlServer("Server=localhost; Database=musicdb; Trusted_Connection=True;TrustServerCertificate=True;"));
builder.Services.AddScoped<TrackRepository>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/tracks", (TrackRepository repo) => repo.GetAll());
app.Run();