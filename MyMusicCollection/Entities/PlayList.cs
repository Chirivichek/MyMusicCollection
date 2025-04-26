namespace MyMusicCollection.Entities
{
    public class PlayList
    {
        public int PlayListId { get; set; } // Primary Key
        public string PlayListName { get; set; }
        public DateTime DateCreated { get; set; }
        public int UserId { get; set; } // Foreign Key

        // Navigation properties
        public User User { get; set; } // Many-to-One relationship with User
        public ICollection<Track> Tracks { get; set; } // Many-to-Many relationship with Track
    }
}
