using System.ComponentModel.DataAnnotations;

namespace MyMusicCollection.Entities
{
    public class Artist
    {
        public int ArtistId { get; set; } // Primary Key
        [MaxLength(250)]
        public string bandName { get; set; }
        [MaxLength(250)]
        public string Country { get; set; }
        [MaxLength(250)]
        public string yearsOfActivity { get; set; }// for exemple, "1990-2000"

        [MaxLength(5000)]
        public string Biography { get; set; }

        // Navigation properties
        public ICollection<Genre> Genres { get; set; } // Many-to-Many relationship with Genre
        public ICollection<Album> Albums { get; set; } // One-to-Many relationship with Album
        public override string ToString()
        {
            return $"{bandName} ({yearsOfActivity}) - {Country}";
        }
    }

}
