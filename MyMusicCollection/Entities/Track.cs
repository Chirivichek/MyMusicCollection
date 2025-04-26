using System.ComponentModel.DataAnnotations;

namespace MyMusicCollection.Entities
{
    public class Track
    {
        public int TrackId { get; set; } // Primary Key
        [MaxLength(250)]
        public string TrackName { get; set; }
        public int Duration { get; set; }
        public int NumberInList { get; set; }
        [MaxLength(250)]
        public string LyricsAuthor { get; set; }
        [MaxLength(250)]
        public string MusicAuthor { get; set; }
        public int AlbumId { get; set; } // Foreign Key
        // Navigation properties
        public Album Album { get; set; } // Many-to-One relationship with Album
        public ICollection<Genre> Genres { get; set; } // Many-to-Many relationship with Genre
        public ICollection<PlayList> PlayLists { get; set; } // Many-to-Many relationship with PlayList
        public override string ToString()
        {
            return $"{TrackName} - Album: {Album?.AlbumName} ({Duration / 60}:{Duration % 60:D2}), Music/Lyrics author - {MusicAuthor}/{LyricsAuthor}";
        }


    }
}
