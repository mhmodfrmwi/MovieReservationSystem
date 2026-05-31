using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Domain.Entities.CinemaModule
{
    public class Cinema : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Location { get; set; }

        // Navigation Properties
        public IList<Hall> Halls { get; set; } = new List<Hall>();
    }
}
