using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusicCollection.Entitys;

namespace MyMusicCollection.Configurations
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