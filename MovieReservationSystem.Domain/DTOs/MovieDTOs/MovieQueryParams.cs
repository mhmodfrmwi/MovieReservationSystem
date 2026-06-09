namespace MovieReservationSystem.Domain.DTOs.MovieDTOs
{
    public class MovieQueryParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? Search { get; set; }
        public string? Genre { get; set; }
        public int? GenreId { get; set; }

        /// <summary>Alias for Genre — used by category dropdown filters.</summary>
        public string? Category
        {
            get => Genre;
            set => Genre = value;
        }

        /// <summary>Alias for GenreId — used when dropdown value is the category id.</summary>
        public int? CategoryId
        {
            get => GenreId;
            set => GenreId = value;
        }
    }
}
