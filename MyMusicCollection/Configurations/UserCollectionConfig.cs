using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusicCollection_Api.Entities;

namespace MyMusicCollection_Api.Configurations
{
    internal class UserCollectionConfig : IEntityTypeConfiguration<UserCollection>
    {
        public void Configure(EntityTypeBuilder<UserCollection> builder)
        {
            // Default value for Status
            builder
                .Property(uc => uc.Status)
                .HasDefaultValue("wanted");

            // Index for DateAdded
            builder
                .HasIndex(uc => uc.DateAdded);

            // Cascading delete: if a User or Album is deleted, the associated UserCollections are deleted
            builder
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCollections)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(uc => uc.Album)
                .WithMany(a => a.UserCollections)
                .HasForeignKey(uc => uc.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}