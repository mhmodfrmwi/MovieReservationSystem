using MovieReservationSystem.Domain.DTOs.DashboardDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Services_Abstraction.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetSystemStatsAsync();
    }
}
