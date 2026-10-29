# Music Collection API

My pet project for learning ASP.NET Core. It's an API for a music collection: you can add tracks, artists and genres, search tracks, upload mp3 files and listen to them in a simple web player.

The project is still in progress (see the To do list below).

## What I used

- C# and .NET 10
- ASP.NET Core Minimal API
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
2. Change the connection string in `appsettings.json` if your SQL Server is not on `localhost`
3. Run `dotnet ef database update` in the `MusicApi` folder to create the database
4. Start the project and open `https://localhost:7022/swagger`

The player is on `https://localhost:7022/`.

## What I learned

- how to set up EF Core, make migrations and work with relations between tables
- why it's better to return DTOs and not database entities
- which status codes to return (201, 204, 404, 409, 401)
- how to store passwords safely with hashing