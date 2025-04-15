using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusicCollection.Entitys;

namespace MyMusicCollection.Configurations
{
    internal class PlayListConfig : IEntityTypeConfiguration<PlayList>
    {
        public void Configure(EntityTypeBuilder<PlayList> builder)
        {
            // Обов'язкове поле PlayListName
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

            // Зв'язок з User (каскадне видалення не додаємо, щоб не видаляти плейлисти при видаленні користувача)
            builder
                .HasOne(p => p.User)
                .WithMany(u => u.PlayLists)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Не видаляти плейлисти при видаленні User

            // Зв'язок Many-to-Many з Track уже визначений у TrackConfig
        }
    }
}