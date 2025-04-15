using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusicCollection.Entitys;

namespace MyMusicCollection.Configurations
{
    internal class RatingAndReviewConfig : IEntityTypeConfiguration<RatingAndReview>
    {
        public void Configure(EntityTypeBuilder<RatingAndReview> builder)
        {
            // Обмеження для Rating (1–5)
            builder
                .Property(r => r.Rating)
                .IsRequired()
                .HasDefaultValue(1)
                .HasAnnotation("CheckConstraint", "Rating >= 1 AND Rating <= 5");

            // Обов'язкове поле Comment
            builder
                .Property(r => r.Comment)
                .IsRequired()
                .HasMaxLength(500);

            // Каскадне видалення: якщо видаляється User або Album, видаляються пов'язані RatingAndReview
            builder
                .HasOne(r => r.User)
                .WithMany(u => u.RatingsAndReviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(r => r.Album)
                .WithMany(a => a.RatingsAndReviews)
                .HasForeignKey(r => r.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}