using MovieReservationSystem.Domain.DTOs.GenreDTOs;
using MovieReservationSystem.Domain.Entities.MovieModule;
using MovieReservationSystem.Domain.Interfaces;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class GenreService(IUnitOfWork unitOfWork) : IGenreService
    {
        public async Task<GenreDto> AddGenreAsync(CreateGenreDto genre)
        {
            var genreEntity = new Genre { Name = genre.Name };
            await unitOfWork.Repository<Genre>().AddAsync(genreEntity);
            await unitOfWork.CompleteAsync();
            return MapToDto(genreEntity);
        }

        public async Task<IEnumerable<GenreDto>> GetAllGenresAsync()
        {
            var genres = await unitOfWork.Repository<Genre>().GetAllAsync();
            return genres.OrderBy(g => g.Name).Select(MapToDto);
        }

        public async Task<GenreDto?> GetGenreByIdAsync(int id)
        {
            var genre = await unitOfWork.Repository<Genre>().GetByIdAsync(id);
            return genre is null ? null : MapToDto(genre);
        }

        public async Task<GenreDto?> UpdateGenreAsync(int id, UpdateGenreDto dto)
        {
            var genre = await unitOfWork.Repository<Genre>().GetByIdAsync(id);
            if (genre is null) return null;

            genre.Name = dto.Name;
            unitOfWork.Repository<Genre>().Update(genre);
            await unitOfWork.CompleteAsync();
            return MapToDto(genre);
        }

        public async Task<bool> DeleteGenreAsync(int id)
        {
            var genre = await unitOfWork.Repository<Genre>().GetByIdAsync(id);
            if (genre is null) return false;

            unitOfWork.Repository<Genre>().Delete(genre);
            await unitOfWork.CompleteAsync();
            return true;
        }

        private static GenreDto MapToDto(Genre genre) => new(genre.Id, genre.Name);
    }
}
