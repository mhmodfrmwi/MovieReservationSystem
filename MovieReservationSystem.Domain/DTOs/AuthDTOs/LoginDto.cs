using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Domain.DTOs.AuthDTOs
{
    public record LoginDto(string Email, string Password);
}
