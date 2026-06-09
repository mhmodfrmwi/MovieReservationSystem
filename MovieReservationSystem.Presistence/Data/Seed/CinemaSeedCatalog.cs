namespace MovieReservationSystem.Presistence.Data.Seed
{
    internal record CinemaSeedEntry(string Name, string Location, HallSeedEntry[] Halls);

    internal record HallSeedEntry(string Name, int Capacity, int SeatsPerRow = 12);

    internal static class CinemaSeedCatalog
    {
        internal static IReadOnlyList<CinemaSeedEntry> All { get; } =
        [
            new("Galaa Cinema Complex", "Nasr City, Cairo, Egypt",
            [
                new("Galaa Premium 1", 144),
                new("Galaa Standard 2", 120),
                new("Galaa IMAX", 200, 16),
                new("Galaa VIP Lounge", 48, 8)
            ]),
            new("Renaissance Cinemas - Mall of Egypt", "6th of October City, Giza, Egypt",
            [
                new("Renaissance Screen 1", 168),
                new("Renaissance Screen 2", 132),
                new("Renaissance Screen 3", 108),
                new("Renaissance 4DX", 96, 12)
            ]),
            new("Point 90 Cinema", "New Cairo, Cairo, Egypt",
            [
                new("Point 90 Hall A", 156),
                new("Point 90 Hall B", 120),
                new("Point 90 Hall C", 96)
            ]),
            new("City Centre Almaza", "Heliopolis, Cairo, Egypt",
            [
                new("Almaza Gold Class", 72, 9),
                new("Almaza Screen 2", 144),
                new("Almaza Screen 3", 120),
                new("Almaza Screen 4", 108)
            ]),
            new("Americana Plaza Cinemas", "Sheikh Zayed, Giza, Egypt",
            [
                new("Americana Platinum", 84, 12),
                new("Americana Screen 2", 132),
                new("Americana Screen 3", 120)
            ]),
            new("Metro Cinema - Alexandria", "Raml Station, Alexandria, Egypt",
            [
                new("Metro Alexandria 1", 180),
                new("Metro Alexandria 2", 144),
                new("Metro Alexandria 3", 108)
            ]),
            new("CineRoyal - Dubai Mall", "Downtown Dubai, UAE",
            [
                new("CineRoyal Platinum", 96, 12),
                new("CineRoyal Screen 2", 168),
                new("CineRoyal Screen 3", 144),
                new("CineRoyal IMAX", 220, 20)
            ]),
            new("VOX Cinemas - Mall of the Emirates", "Al Barsha, Dubai, UAE",
            [
                new("VOX Gold", 72, 9),
                new("VOX Screen 5", 156),
                new("VOX Screen 6", 132),
                new("VOX MAX", 192, 16)
            ]),
            new("AMC Empire 25", "Times Square, New York, USA",
            [
                new("Empire Screen 1", 200, 20),
                new("Empire Screen 2", 168),
                new("Empire Screen 3", 144)
            ]),
            new("Odeon Leicester Square", "West End, London, UK",
            [
                new("Odeon Luxe", 120, 12),
                new("Odeon Screen 2", 156),
                new("Odeon Screen 3", 132)
            ])
        ];
    }
}
