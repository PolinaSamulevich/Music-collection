using MusicApi.Context.Models;
using MusicApi.Context.Repositories;
using MusicApi.Services.Models;

namespace MusicApi.Services
{
    public interface IArtistService
    {
        ArtistDto? GetByIdWithTracks(int id);
        ArtistDto Create(ArtistRequest request);
    }

    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artists;

        public ArtistService(IArtistRepository artists)
        {
            _artists = artists;
        }

        public ArtistDto? GetByIdWithTracks(int id)
        {
            var artist = _artists.GetByIdWithTracks(id);
            if (artist == null)
                return null;

            // Исполнитель у всех его треков один и тот же — проставляем явно,
            // т.к. навигацию Track.Artist репозиторий не загружает.
            foreach (var track in artist.Tracks)
                track.Artist = artist;

            return new ArtistDto(artist.Id, artist.Name, artist.Tracks.Select(t => t.ToDto()).ToList());
        }

        public ArtistDto Create(ArtistRequest request)
        {
            var artist = new Artist { Name = request.Name };
            _artists.Add(artist);
            return new ArtistDto(artist.Id, artist.Name, new List<TrackDto>());
        }
    }
}
