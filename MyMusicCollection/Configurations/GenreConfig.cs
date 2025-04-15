using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusicCollection.Entitys;

namespace MyMusicCollection.Configurations
{
    internal class GenreConfig : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            // Обов'язкове поле GenreName
            builder
                .Property(g => g.GenreName)
                .IsRequired()
                .HasMaxLength(50);

            // Унікальність GenreName
            builder
                .HasIndex(g => g.GenreName)
                .IsUnique();
        }
    }
}