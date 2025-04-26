using System.ComponentModel.DataAnnotations;

namespace MyMusicCollection.Entities
{
    public class Album
    {
        public int AlbumId { get; set; } // Primary Key
        [MaxLength(250)]
        public string AlbumName { get; set; }
        public int AlbumDuration { get; set; }
        public int TrackCount { get; set; }
        [MaxLength(250)]
        public string Label { get; set; }
        [MaxLength(250)]
        public string Format { get; set; } // CD, Vinyl, Digital 
        public DateTime ReleaseDate { get; set; }
        public int ArtistId { get; set; } // Foreign Key

        // Navigation properties
        public Artist Artist { get; set; } // Many-to-One relationship with Artist
        public ICollection<Genre> Genres { get; set; } // Many-to-Many relationship with Genre
        public ICollection<Track> Tracks { get; set; } // One-to-Many relationship with Track
        public ICollection<RatingAndReview> RatingsAndReviews { get; set; }
        public ICollection<UserCollection> UserCollections { get; set; }
    }

}
