using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusicCollection.Entitys;

namespace MyMusicCollection.Configurations
{
    internal class ArtistConfig
    {
        public void Configure(EntityTypeBuilder<Artist> builder)
        {
            // Обов'язкове поле bandName
            builder
                .Property(a => a.bandName)
                .IsRequired()
                .HasMaxLength(100);

            // Унікальність bandName
            builder
                .HasIndex(a => a.bandName)
                .IsUnique();

            // Каскадне видалення: якщо видаляється Artist, видаляються всі пов'язані Album
            builder
                .HasMany(a => a.Albums)
                .WithOne(a => a.Artist)
                .HasForeignKey(a => a.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);

            // Зв'язок Many-to-Many з Genre через ArtistGenre
            builder
                .HasMany(a => a.Genres)
                .WithMany(g => g.Artists)
                .UsingEntity("ArtistGenre");
        }
    }
}
