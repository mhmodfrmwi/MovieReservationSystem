using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Domain.DTOs.ReviewDTOs
{
    public class CreateReviewDto
    {
        public int MovieId { get; set; }
        public int Rating { get; set; } 
        public string? Comment { get; set; }
    }
}
