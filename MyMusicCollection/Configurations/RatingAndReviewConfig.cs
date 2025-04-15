using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusicCollection.Entitys;

namespace MyMusicCollection.Configurations
{
    internal class RatingAndReviewConfig : IEntityTypeConfiguration<RatingAndReview>
    {
        public void Configure(EntityTypeBuilder<RatingAndReview> builder)
        {
            // Limits for Rating (1–5)
            builder
                .Property(r => r.Rating)
                .IsRequired()
                .HasDefaultValue(1)
                .HasAnnotation("CheckConstraint", "Rating >= 1 AND Rating <= 5");

            // Required Comment field
            builder
                .Property(r => r.Comment)
                .IsRequired()
                .HasMaxLength(500);

            // Cascading delete: if User or Album is deleted, the related RatingAndReview are deleted
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