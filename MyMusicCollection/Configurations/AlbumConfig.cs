using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusicCollection.Entitys;

namespace MyMusicCollection.Configurations
{
    internal class AlbumConfig : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            // Обов'язкове поле AlbumName
            builder
                .Property(a => a.AlbumName)
                .IsRequired()
                .HasMaxLength(100);

            // Унікальність AlbumName для одного Artist
            builder
                .HasIndex(a => new { a.ArtistId, a.AlbumName })
                .IsUnique();

            // Індекс для ReleaseDate (вже є)
            builder
                .HasIndex(a => a.ReleaseDate);

            // Каскадне видалення: якщо видаляється Album, видаляються всі пов'язані Track
            builder
                .HasMany(a => a.Tracks)
                .WithOne(t => t.Album)
                .HasForeignKey(t => t.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            // Зв'язок Many-to-Many з Genre через AlbumGenre
            builder
                .HasMany(a => a.Genres)
                .WithMany(g => g.Albums)
                .UsingEntity("AlbumGenre");
        }
    }
}