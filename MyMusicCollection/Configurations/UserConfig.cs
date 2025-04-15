using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusicCollection.Entitys;

namespace MyMusicCollection.Configurations
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Обов'язкове поле Username
            builder
                .Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);

            // Унікальність Username
            builder
                .HasIndex(u => u.UserName)
                .IsUnique();
        }
    }
}