using MovieReservationSystem.Domain.DTOs.CinemaDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Services_Abstraction.Interfaces
{
    public interface ICinemaService
    {
        Task<IEnumerable<CinemaDto>> GetAllCinemasAsync();
        Task<CinemaDto?> GetCinemaByIdAsync(int id);
        Task<CinemaDto> AddCinemaAsync(CreateCinemaDto dto);
    }
}
