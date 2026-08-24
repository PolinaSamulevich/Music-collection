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
app.MapGet("/tracks'{id}", (TrackRepository repo, int id) =>
{
    var track = repo.GetById(id);
    if (track == null)
        return Results.Problem(title: "track is not found", statusCode: 404);
    return Results.Ok(track);
})
.WithSummary("get track by id");
app.MapPost("/tracks", (TrackRepository repo, Track newTrack) =>
{
    repo.Add(newTrack);
    return Results.Ok(newTrack);

}).WithSummary("add a new track");
app.MapPut("/tracks,{id}", (TrackRepository repo, int id, Track updatedTrack) =>
{
    repo.Update(id, updatedTrack);
    return Results.Ok($"track is updated");
}).WithSummary("update track");
app.MapDelete("/tracks{id}", (TrackRepository repo, int id) =>
{
    repo.Delete(id);
    if (id == null)
        return Results.NotFound("track id={id} is not found");
    return Results.Ok("track is deleted");
}).WithSummary("delete track");
app.Run();