namespace MovieReservationSystem.Domain.DTOs.HallDTOs
{
    public record CreateHallDto(string Name, int Capacity, int CinemaId);
}
