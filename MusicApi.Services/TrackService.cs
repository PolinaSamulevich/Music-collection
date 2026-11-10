using Microsoft.Extensions.Logging;
using MusicApi.Context.Models;
using MusicApi.Context.Repositories;
using MusicApi.Services.Models;

namespace MusicApi.Services
{
    public interface ITrackService
    {
        List<TrackDto> GetAll();
        TrackDto? GetById(int id);
        List<TrackDto> Search(string query);
        TrackDto Create(TrackRequest request);
        /// <returns>false, если трек не найден.</returns>
        bool Update(int id, TrackRequest request);
        /// <returns>false, если трек не найден.</returns>
        bool Delete(int id);
        /// <returns>Обновлённый трек или null, если трек не найден.</returns>
        TrackDto? SetAudioUrl(int id, string audioUrl);
    }

    public class TrackService : ITrackService
    {
        private readonly ITrackRepository _tracks;
        private readonly ILogger<TrackService> _logger;

        public TrackService(ITrackRepository tracks, ILogger<TrackService> logger)
        {
            _tracks = tracks;
            _logger = logger;
        }

        public List<TrackDto> GetAll() =>
            _tracks.GetAll().Select(t => t.ToDto()).ToList();

        public TrackDto? GetById(int id)
        {
            var track = _tracks.GetById(id);
            if (track == null)
            {
                _logger.LogWarning("The track with id={Id} was not found", id);
                return null;
            }
            return track.ToDto();
        }

        public List<TrackDto> Search(string query) =>
            _tracks.Search(query).Select(t => t.ToDto()).ToList();

        public TrackDto Create(TrackRequest request)
        {
            var track = new Track
            {
                Title = request.Title,
                ArtistId = request.ArtistId,
                GenreId = request.GenreId,
                DurationSeconds = request.DurationSeconds
            };
            _tracks.Add(track);
            _logger.LogInformation("added track {Title}", track.Title);

            // перечитываем, чтобы подтянулись Artist и Genre для DTO
            return (_tracks.GetById(track.Id) ?? track).ToDto();
        }

        public bool Update(int id, TrackRequest request)
        {
            var track = _tracks.GetById(id);
            if (track == null)
                return false;

            track.Title = request.Title;
            track.DurationSeconds = request.DurationSeconds;
            track.ArtistId = request.ArtistId;
            track.GenreId = request.GenreId;
            _tracks.Update(track);
            return true;
        }

        public bool Delete(int id)
        {
            var track = _tracks.GetById(id);
            if (track == null)
                return false;

            _tracks.Delete(track);
            _logger.LogInformation("deleted track {Id}. {Title}", id, track.Title);
            return true;
        }

        public TrackDto? SetAudioUrl(int id, string audioUrl)
        {
            var track = _tracks.GetById(id);
            if (track == null)
                return null;

            track.AudioUrl = audioUrl;
            _tracks.Update(track);
            return track.ToDto();
        }
    }
}
