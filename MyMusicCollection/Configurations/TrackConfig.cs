using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusicCollection.Entitys;

namespace MyMusicCollection.Configurations
{
    internal class TrackConfig : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> builder)
        {
            // Required TrackName field
            builder
             .HasMany(t => t.PlayLists)
             .WithMany(p => p.Tracks)
             .UsingEntity<Dictionary<string, object>>(
                 "PlaylistTrack",
                 j => j
             .HasOne<PlayList>()
             .WithMany()
             .HasForeignKey("PlayListId")
             .OnDelete(DeleteBehavior.Cascade),
                j => j
             .HasOne<Track>()
             .WithMany()
             .HasForeignKey("TrackId")
             .OnDelete(DeleteBehavior.NoAction),
                 j =>
                 {
             j.HasKey("PlayListId", "TrackId");
                 });
        }
    }
}