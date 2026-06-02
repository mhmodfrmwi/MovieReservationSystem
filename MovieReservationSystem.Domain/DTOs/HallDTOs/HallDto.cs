using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Domain.DTOs.HallDTOs
{
    public record HallDto(int Id, string Name, int Capacity, int CinemaId);
}
