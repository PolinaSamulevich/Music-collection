# Music Collection API

My pet project for learning ASP.NET Core. It's an API for a music collection: you can add tracks, artists and genres, search tracks, upload mp3 files and listen to them in a simple web player.

The project is still in progress (see the To do list below).

## What I used

- C# and .NET 10
- ASP.NET Core Web API (controllers), three-layer architecture
- Entity Framework Core + SQL Server
- Swagger for testing the endpoints
- HTML, CSS and JavaScript for the player page

## What it can do

- add, edit, delete and get tracks
- add artists and genres
- search tracks by title
- show an artist with all their tracks
- upload an mp3 file for a track
- register and log in (passwords are saved as hashes, not plain text)

## Architecture

```
MusicApi.WebApi  ->  MusicApi.Services  ->  MusicApi.Context
controllers, DI      business logic, DTOs   DbContext, entities, migrations, repositories
```

- **MusicApi.WebApi** – controllers, `Program.cs` (DI, Swagger, static files), `wwwroot`
- **MusicApi.Services** – services (`TrackService`, `ArtistService`, `GenreService`, `AuthService`), DTOs and request models with validation
- **MusicApi.Context** – everything about the database: `MusicdbContext`, entities, repositories, EF migrations

## Endpoints

```
GET    /tracks
GET    /tracks/{id}
GET    /tracks/search?query=...
POST   /tracks
PUT    /tracks/{id}
DELETE /tracks/{id}
POST   /tracks/{id}/upload

GET    /artists/{id}
POST   /artists
POST   /Genres

POST   /auth/register
POST   /auth/login
```

## How to run

1. Clone the repo
2. Change the connection string in `MusicApi.WebApi/appsettings.json` if your SQL Server is not on `localhost`
3. Run `dotnet ef database update --project MusicApi.Context --startup-project MusicApi.WebApi` in the repo root to create the database
4. Start the project and open `https://localhost:7022/swagger`

The player is on `https://localhost:7022/`.

## What I learned

- how to set up EF Core, make migrations and work with relations between tables
- why it's better to return DTOs and not database entities
- which status codes to return (201, 204, 404, 409, 401)
- how to store passwords safely with hashing