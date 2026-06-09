namespace MovieReservationSystem.Domain.DTOs.GenreDTOs
{
    /// <summary>
    /// Dropdown-friendly shape: supports both id/name and value/label bindings.
    /// </summary>
    public record CategoryDropdownDto(int Id, string Name)
    {
        public int Value => Id;
        public string Label => Name;
    }
}
