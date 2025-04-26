using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusicCollection.Entities;

namespace MyMusicCollection.Configurations
{
    internal class ArtistConfig
    {
        public void Configure(EntityTypeBuilder<Artist> builder)
        {
            // Required field bandName
            builder
                .Property(a => a.bandName)
                .IsRequired()
                .HasMaxLength(100);

            // Uniqueness of bandName
            builder
                .HasIndex(a => a.bandName)
                .IsUnique();

            // Cascading delete: if an Artist is deleted, all related Albums are deleted
            builder
                .HasMany(a => a.Albums)
                .WithOne(a => a.Artist)
                .HasForeignKey(a => a.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-Many relationship with Genre via ArtistGenre
            builder
                .HasMany(a => a.Genres)
                .WithMany(g => g.Artists)
                .UsingEntity("ArtistGenre");
        }
    }
}
