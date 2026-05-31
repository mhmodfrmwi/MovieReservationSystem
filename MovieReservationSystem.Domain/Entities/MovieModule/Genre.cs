using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Domain.Entities.MovieModule
{
    public class Genre : BaseEntity<int>
    {
        public string Name { get; set; }
        public IList<Movie> Movies { get; set; } = new List<Movie>();
    }
}
