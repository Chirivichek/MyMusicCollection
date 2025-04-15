using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusicCollection.Entitys;

namespace MyMusicCollection.Configurations
{
    internal class PlayListConfig : IEntityTypeConfiguration<PlayList>
    {
        public void Configure(EntityTypeBuilder<PlayList> builder)
        {
            // Required field PlayListName
            builder
                .Property(p => p.PlayListName)
                .IsRequired()
                .HasMaxLength(100);

            // Унікальність PlayListName для одного User
            builder
                .HasIndex(p => new { p.UserId, p.PlayListName })
                .IsUnique();

            // Індекс для DateCreated
            builder
                .HasIndex(p => p.DateCreated);

            // Connection with User (we do not add cascading deletion so as not to delete playlists when deleting the user)
            builder
                .HasOne(p => p.User)
                .WithMany(u => u.PlayLists)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Do not delete playlists when deleting User
            // Many-to-Many relationship with Track already defined in TrackConfig
        }
    }
}