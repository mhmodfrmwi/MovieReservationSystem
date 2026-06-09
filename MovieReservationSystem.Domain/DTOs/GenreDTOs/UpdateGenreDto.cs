using System.ComponentModel.DataAnnotations;

namespace MovieReservationSystem.Domain.DTOs.GenreDTOs
{
    public class UpdateGenreDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
