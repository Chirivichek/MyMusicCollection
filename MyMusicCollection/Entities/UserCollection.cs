using System.ComponentModel.DataAnnotations;

namespace MyMusicCollection_Api.Entities
{
    public class UserCollection
    {
        public int UserCollectionId { get; set; } // Primary Key
        public DateTime DateAdded { get; set; }
        [MaxLength(50)]
        public string Status { get; set; } // bought, wanted
        public int AlbumId { get; set; } // Foreign Key
        public int UserId { get; set; } // Foreign Key
        // Navigation properties
        public Album Album { get; set; } // Many-to-One relationship with Album
        public User User { get; set; } // Many-to-One relationship with User
    }
}
