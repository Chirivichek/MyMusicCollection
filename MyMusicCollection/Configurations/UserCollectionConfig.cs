using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusicCollection.Entitys;

namespace MyMusicCollection.Configurations
{
    internal class UserCollectionConfig : IEntityTypeConfiguration<UserCollection>
    {
        public void Configure(EntityTypeBuilder<UserCollection> builder)
        {
            // Значення за замовчуванням для Status
            builder
                .Property(uc => uc.Status)
                .HasDefaultValue("wanted");

            // Індекс для DateAdded
            builder
                .HasIndex(uc => uc.DateAdded);

            // Каскадне видалення: якщо видаляється User або Album, видаляються пов'язані UserCollection
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