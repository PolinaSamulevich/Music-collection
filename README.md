# Music Collection API

REST API for managing a personal music library: tracks, artists and genres, with search, MP3 upload and a built-in web player.

> Personal learning project, in progress. See [Roadmap](#roadmap).

<!-- TODO: add a screenshot of the player, e.g. ![Player](docs/player.png) -->

## Tech stack

- C#, .NET 10, ASP.NET Core (Minimal APIs)
- Entity Framework Core (code-first, migrations), SQL Server, LINQ
- Swagger / OpenAPI
- Password hashing with ASP.NET Core Identity `PasswordHasher`
- HTML, CSS, JavaScript (web player and admin page in `wwwroot`)

## Features

- CRUD for tracks, adding artists and genres
- Search tracks by title
- Artist page with all their tracks
- MP3 upload for a track, played in the web player
- User registration and login (passwords stored as salted hashes)
- Input validation with data annotations
- Correct HTTP status codes: 201, 204, 404, 409, 401

## API endpoints

| Method | Route | Description |
|---|---|---|
| GET | `/tracks` | List all tracks |
| GET | `/tracks/{id}` | Get a track by id |
| GET | `/tracks/search?query=` | Search tracks by title |
| POST | `/tracks` | Add a track (201 Created) |
| PUT | `/tracks/{id}` | Update a track (204 / 404) |
| DELETE | `/tracks/{id}` | Delete a track (204 / 404) |
| POST | `/tracks/{id}/upload` | Upload an MP3 file for a track |
| GET | `/artists/{id}` | Get an artist with their tracks |
| POST | `/artists` | Add an artist |
| POST | `/Genres` | Add a genre |
| POST | `/auth/register` | Register a user (201 / 409) |
| POST | `/auth/login` | Log in (200 / 401) |

Full interactive documentation is available in Swagger UI when the app runs.

## Database

Four tables: `Tracks`, `Artists`, `Genres`, `Users`.

- An artist has many tracks, a genre has many tracks.
- `Users.Email` has a unique index.

## Project structure

```
MusicApi/
├── Program.cs        # app setup and endpoints
├── Data/             # MusicdbContext (EF Core)
├── Models/           # entities, DTOs and request models
├── Repositories/     # data access (Track, Artist, User)
├── Migrations/       # EF Core migrations
└── wwwroot/          # web player, admin page, uploaded audio
```

## Getting started

**Requirements:** .NET 10 SDK, SQL Server (or SQL Server Express).

1. Clone the repository:
   ```
   git clone https://github.com/PolinaSamulevich/Music-collection.git
   cd Music-collection/MusicApi
   ```
2. Check the connection string `MusicDb` in `appsettings.json`. By default it uses `Server=localhost` with Windows authentication.
3. Create the database:
   ```
   dotnet tool install --global dotnet-ef
   dotnet ef database update
   ```
4. Run the app:
   ```
   dotnet run --launch-profile https
   ```
5. Open:
   - Web player: https://localhost:7022/
   - Admin page: https://localhost:7022/admin.html
   - Swagger UI: https://localhost:7022/swagger

## Roadmap

- [x] CRUD for tracks, artists and genres
- [x] Search and MP3 upload
- [x] EF Core migrations
- [x] User registration and login
- [ ] JWT tokens and roles (Admin / User)
- [ ] Favourite tracks
- [ ] Unit and integration tests (xUnit)
- [ ] Docker

## Author

Palina Samulevich · [GitHub](https://github.com/PolinaSamulevich)
