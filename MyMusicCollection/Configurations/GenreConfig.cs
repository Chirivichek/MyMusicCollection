using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusicCollection_Api.Entities;

namespace MyMusicCollection_Api.Configurations
{
    internal class GenreConfig : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            // Required field GenreName
            builder
                .Property(g => g.GenreName)
                .IsRequired()
                .HasMaxLength(50);

            // Uniqueness of GenreName
            builder
                .HasIndex(g => g.GenreName)
                .IsUnique();
        }
    }
}