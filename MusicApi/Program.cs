using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Models;
using MusicApi.Repositories;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidation();
builder.Services.AddDbContext<MusicdbContext>(options =>
    options.UseSqlServer("Server=localhost; Database=musicdb; Trusted_Connection=True;TrustServerCertificate=True;"));
builder.Services.AddScoped<TrackRepository>();
builder.Services.AddScoped<ArtistRepository>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();   
}
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapGet("/", (IWebHostEnvironment env) =>
    Results.File(Path.Combine(env.ContentRootPath, "wwwroot", "index.html"), "text/html; charset=utf-8"));
app.MapGet("/tracks", (TrackRepository repo) => 
{
    var result = repo.GetAll()
    .Select(t => new TrackDto(
        t.Id,
        t.Title,
        t.Artist?.Name ?? "Untitled",
        t.Genre?.Name ?? "Untitled",
        t.DurationFormatted,
        t.AudioUrl));
    return Results.Ok(result);
    });
app.MapGet("/tracks/{id}", (TrackRepository repo, int id) =>
{
    var track = repo.GetById(id);
    if (track == null)
        return Results.Problem(title: "track is not found", statusCode: 404);
    return Results.Ok(track);
}).WithSummary("get track by id");
app.MapGet("/artists/{id}", (int id, ArtistRepository repo) =>
{
    var artist = repo.GetByIdWithTracks(id);
    if (artist == null)
        return Results.NotFound($"artist id={id} is not found");
    return Results.Ok(artist);
}).WithSummary("get artist and tracks");
app.MapGet("/tracks/search", (string query, TrackRepository repo) =>
{
    var results = repo.Search(query);
    if (results == null)
        return Results.NotFound("song is not found");
    return Results.Ok(results);
}).WithSummary("search track by name");
app.MapPost("/artists", (Artist newArtist, MusicdbContext db) =>
{
    db.Artists.Add(newArtist);
    db.SaveChanges();
    return Results.Ok($"artist {newArtist.Name} is added");
}).WithSummary("add a new artists");
app.MapPost("/tracks", (TrackRepository repo, Track newTrack) =>
{
    repo.Add(newTrack);
    return Results.Created($"/tracks/{newTrack.Title}", newTrack);

}).WithSummary("add a new track");
app.MapPost("/Genres", (MusicdbContext db, Genre newGenre) =>
{
    db.Genres.Add(newGenre);
    db.SaveChanges();
    return Results.Ok($"genre {newGenre.Name} is added");
}).WithSummary("add a new genre");
app.MapPost("/tracks/{id}/upload", async (int id, IFormFile file, TrackRepository repo) =>
{
    var track = repo.GetById(id);
    if (track == null)
        return Results.NotFound("track is not found");
    var filePath = Path.Combine("wwwroot/audio", $"{id}.mp3");
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }
    track.AudioUrl = $"/audio/{id}.mp3";
    repo.SaveChanges();
    return Results.Ok(track);
})
    .DisableAntiforgery();
app.MapPut("/tracks/{id}", (TrackRepository repo, int id, Track updatedTrack) =>
{
    var updTrack = repo.GetById(id);
    if (updTrack == null)
       return Results.NotFound($"track {id} is not found");
    repo.Update(id, updatedTrack);
    return Results.NoContent();
}).WithSummary("update track");
app.MapDelete("/tracks/{id}", (TrackRepository repo, int id) =>
{
    var track = repo.GetById(id);
    if (track == null)
        return Results.NotFound($"track id={id} is not found");
    repo.Delete(id);
    return Results.NoContent();
}).WithSummary("delete track");
app.Run();