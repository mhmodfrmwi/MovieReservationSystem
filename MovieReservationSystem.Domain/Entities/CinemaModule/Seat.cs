using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Domain.Entities.CinemaModule
{
    public class Seat : BaseEntity<int>
    {
        public string Row { get; set; } 
        public int Number { get; set; } 
        public string Code => $"{Row}{Number}"; 

        public int HallId { get; set; } 

        // Navigation Properties
        public Hall Hall { get; set; }
    }
}
