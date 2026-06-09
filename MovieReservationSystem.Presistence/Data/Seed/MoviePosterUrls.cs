namespace MovieReservationSystem.Presistence.Data.Seed
{
    /// <summary>
    /// Stable Wikimedia poster URLs (TMDB hotlinks often 404).
    /// </summary>
    internal static class MoviePosterUrls
    {
        internal static IReadOnlyDictionary<string, string> ByTitle { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Inception"] = "https://upload.wikimedia.org/wikipedia/en/2/2e/Inception_%282010%29_theatrical_poster.jpg",
                ["The Dark Knight"] = "https://upload.wikimedia.org/wikipedia/en/1/1c/The_Dark_Knight_%282008_film%29.jpg",
                ["Interstellar"] = "https://upload.wikimedia.org/wikipedia/en/b/bc/Interstellar_film_poster.jpg",
                ["The Shawshank Redemption"] = "https://upload.wikimedia.org/wikipedia/en/8/81/ShawshankRedemptionMoviePoster.jpg",
                ["Pulp Fiction"] = "https://upload.wikimedia.org/wikipedia/en/3/3b/Pulp_Fiction_%281994%29_poster.jpg",
                ["Forrest Gump"] = "https://upload.wikimedia.org/wikipedia/en/6/67/Forrest_Gump_poster.jpg",
                ["The Godfather"] = "https://upload.wikimedia.org/wikipedia/en/1/1c/Godfather_ver1.jpg",
                ["Fight Club"] = "https://upload.wikimedia.org/wikipedia/en/f/fc/Fight_Club_poster.jpg",
                ["The Matrix"] = "https://upload.wikimedia.org/wikipedia/en/d/db/The_Matrix.png",
                ["Gladiator"] = "https://upload.wikimedia.org/wikipedia/en/f/fb/Gladiator_%282000_film_poster%29.png",
                ["Titanic"] = "https://upload.wikimedia.org/wikipedia/en/1/18/Titanic_%281997_film%29_poster.png",
                ["Avatar"] = "https://upload.wikimedia.org/wikipedia/en/d/d6/Avatar_%282009_film%29_poster.jpg",
                ["Avengers: Endgame"] = "https://upload.wikimedia.org/wikipedia/en/0/0d/Avengers_Endgame_poster.jpg",
                ["Spider-Man: No Way Home"] = "https://upload.wikimedia.org/wikipedia/en/0/00/Spider-Man_No_Way_Home_poster.jpg",
                ["Joker"] = "https://upload.wikimedia.org/wikipedia/en/e/e1/Joker_%282019_film%29_poster.jpg",
                ["Parasite"] = "https://upload.wikimedia.org/wikipedia/en/5/53/Parasite_%282019_film%29.png",
                ["Oppenheimer"] = "https://upload.wikimedia.org/wikipedia/en/4/4a/Oppenheimer_%28film%29.jpg",
                ["Barbie"] = "https://upload.wikimedia.org/wikipedia/en/0/0b/Barbie_2023_poster.jpg",
                ["Dune"] = "https://upload.wikimedia.org/wikipedia/en/8/8e/Dune_%282021_film%29.jpg",
                ["Top Gun: Maverick"] = "https://upload.wikimedia.org/wikipedia/en/7/65/Top_Gun_Maverick.jpg",
                ["La La Land"] = "https://upload.wikimedia.org/wikipedia/en/a/a4/La_La_Land_%28film%29.png",
                ["Whiplash"] = "https://upload.wikimedia.org/wikipedia/en/0/01/Whiplash_poster.jpg",
                ["The Lion King"] = "https://upload.wikimedia.org/wikipedia/en/9/9d/Disney_The_Lion_King_2019.jpg",
                ["Finding Nemo"] = "https://upload.wikimedia.org/wikipedia/en/2/29/Finding_Nemo.jpg",
                ["Toy Story"] = "https://upload.wikimedia.org/wikipedia/en/1/13/Toy_Story.jpg",
                ["The Conjuring"] = "https://upload.wikimedia.org/wikipedia/en/b/b9/The_Conjuring_poster.jpg",
                ["Get Out"] = "https://upload.wikimedia.org/wikipedia/en/a/a3/Get_Out_poster.png",
                ["The Silence of the Lambs"] = "https://upload.wikimedia.org/wikipedia/en/8/86/The_Silence_of_the_Lambs_poster.jpg",
                ["Se7en"] = "https://upload.wikimedia.org/wikipedia/en/6/68/Seven_%28movie%29_poster.jpg",
                ["The Notebook"] = "https://upload.wikimedia.org/wikipedia/en/8/86/Posternotebook.jpg",
                ["Crazy Rich Asians"] = "https://upload.wikimedia.org/wikipedia/en/1/17/Crazy_Rich_Asians_%28film%29.png",
                ["Knives Out"] = "https://upload.wikimedia.org/wikipedia/en/1/1f/Knives_Out_poster.jpeg",
                ["John Wick"] = "https://upload.wikimedia.org/wikipedia/en/3/3a/John_Wick_%282014_film%29_poster.jpg",
                ["Mad Max: Fury Road"] = "https://upload.wikimedia.org/wikipedia/en/6/6e/Mad_Max_Fury_Road.jpg",
                ["Black Panther"] = "https://upload.wikimedia.org/wikipedia/en/d/d6/Black_Panther_%28film%29_poster.jpg",
                ["Iron Man"] = "https://upload.wikimedia.org/wikipedia/en/5/5c/Iron_Man_poster.jpg",
                ["Guardians of the Galaxy"] = "https://upload.wikimedia.org/wikipedia/en/2/2c/Guardians_of_the_Galaxy_%28film%29_poster.jpg",
                ["The Grand Budapest Hotel"] = "https://upload.wikimedia.org/wikipedia/en/a/a6/The_Grand_Budapest_Hotel_Poster.jpg",
                ["Django Unchained"] = "https://upload.wikimedia.org/wikipedia/en/8/8b/Django_Unchained_Poster.jpg",
                ["The Wolf of Wall Street"] = "https://upload.wikimedia.org/wikipedia/en/d/d8/The_Wolf_of_Wall_Street_%282013%29.png",
                ["Arrival"] = "https://upload.wikimedia.org/wikipedia/en/d/df/Arrival%2C_Movie_Poster.jpg",
                ["Blade Runner 2049"] = "https://upload.wikimedia.org/wikipedia/en/9/9b/Blade_Runner_2049_poster.png",
                ["Harry Potter and the Sorcerer's Stone"] = "https://upload.wikimedia.org/wikipedia/en/d/d0/Harry_Potter_and_the_Philosopher%27s_Stone_%28film%29.jpg",
                ["The Lord of the Rings: The Fellowship of the Ring"] = "https://upload.wikimedia.org/wikipedia/en/f/fb/Lord_Rings_Fellowship_Ring.jpg",
                ["Star Wars: A New Hope"] = "https://upload.wikimedia.org/wikipedia/en/8/87/StarWarsMoviePoster1977.jpg",
                ["The Departed"] = "https://upload.wikimedia.org/wikipedia/en/5/50/Departed234.jpg",
                ["Good Will Hunting"] = "https://upload.wikimedia.org/wikipedia/en/b/b8/Good_Will_Hunting_theatrical_poster.jpeg",
                ["The Prestige"] = "https://upload.wikimedia.org/wikipedia/en/d/d2/Prestige_poster.jpg",
                ["A Quiet Place"] = "https://upload.wikimedia.org/wikipedia/en/5/5a/Quiet_Place_film.jpg",
                ["Inside Out"] = "https://upload.wikimedia.org/wikipedia/en/0/0a/Inside_Out_%282015_film%29_poster.jpg",
                ["The Social Network"] = "https://upload.wikimedia.org/wikipedia/en/8/8c/The_Social_Network_film_poster.png",
                ["The Revenant"] = "https://upload.wikimedia.org/wikipedia/en/b/b6/The_Revenant_2015_film_poster.jpg",
            };
    }
}
