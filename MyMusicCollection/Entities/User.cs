using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MyMusicCollection.Entities
{
    public class User
    {
        public int UserId { get; set; } // Primary Key
        [MaxLength(250)]
        public string UserName { get; set; }
        [MaxLength(250)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        // Navigation properties
        public ICollection<PlayList> PlayLists { get; set; }
        public ICollection<RatingAndReview> RatingsAndReviews { get; set; }
        public ICollection<UserCollection> UserCollections { get; set; }

        public User()
        {
        }
        public override string ToString()
        {
            return $"User ID: {UserId}, Username: {UserName}, Email: {Email}, Date of Birth: {DateOfBirth.ToShortDateString()}";
        }
    }
}
