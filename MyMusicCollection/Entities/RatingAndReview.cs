using System.ComponentModel.DataAnnotations;

namespace MyMusicCollection_Api.Entities
{
    public class RatingAndReview
    {
        public int RatingAndReviewId { get; set; } // Primary Key
        public int Rating { get; set; } // 1 to 5
        [MaxLength(5000)]
        public string Comment { get; set; }
        public int UserId { get; set; } // Foreign Key
        public int AlbumId { get; set; } // Foreign Key
        // Navigation properties
        public User User { get; set; } // Many-to-One relationship with User
        public Album Album { get; set; } // Many-to-One relationship with Album
    }
}
