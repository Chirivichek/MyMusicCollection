namespace MyMusicCollection.Entitys
{
    public class Genre
    {
        public int GenreId { get; set; } // Primary Key
        public string GenreName { get; set; }
        // Navigation properties
        public ICollection<Album> Albums { get; set; } // Many-to-Many
        public ICollection<Artist> Artists { get; set; } // Many-to-Many
        public ICollection<Track> Tracks { get; set; } // Many-to-Many
        public override string ToString()
        {
            return GenreName;
        }
    }

}
