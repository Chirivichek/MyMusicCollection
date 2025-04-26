using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusicCollection.Entities;

namespace MyMusicCollection.Configurations
{
    internal class AlbumConfig : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            // Required field AlbumName
            builder
                .Property(a => a.AlbumName)
                .IsRequired()
                .HasMaxLength(100);

            // Uniqueness of AlbumName for one Artist
            builder
                .HasIndex(a => new { a.ArtistId, a.AlbumName })
                .IsUnique();

            // Index for ReleaseDate 
            builder
                .HasIndex(a => a.ReleaseDate);

            // Cascading delete: if an Album is deleted, all related Tracks are deleted
            builder
                .HasMany(a => a.Tracks)
                .WithOne(t => t.Album)
                .HasForeignKey(t => t.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-Many relationship with Genre via AlbumGenre
            builder
                .HasMany(a => a.Genres)
                .WithMany(g => g.Albums)
                .UsingEntity("AlbumGenre");
        }
    }
}