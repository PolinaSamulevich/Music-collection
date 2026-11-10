using Microsoft.EntityFrameworkCore;
using MusicApi.Context;
using MusicApi.Context.Repositories;
using MusicApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Context
builder.Services.AddDbContext<MusicdbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Musicdb")));
builder.Services.AddScoped<ITrackRepository, TrackRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
