using System.ComponentModel.DataAnnotations;

namespace MovieReservationSystem.Domain.DTOs.GenreDTOs
{
    public class CreateGenreDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
