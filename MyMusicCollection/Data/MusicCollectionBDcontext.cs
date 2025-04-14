using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyMusicCollection.Entitys;
using System.Configuration;

namespace MyMusicCollection.Data
{
    public class MusicCollectionBDcontext : DbContext
    {
        // entities : album, track, artist, userCollection, ratingsAndReviews, Playlists

        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<RatingAndReview> RatingsAndReviews { get; set; }
        public DbSet<PlayList> PlayLists { get; set; }
        public DbSet<UserCollection> UserCollections { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<User> Users { get; set; }


        public MusicCollectionBDcontext(DbContextOptions<MusicCollectionBDcontext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MusicCollectionDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            optionsBuilder.UseSqlServer(connectionString);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayList>()
              .HasMany(p => p.Tracks)
              .WithMany(t => t.PlayLists)
              .UsingEntity<Dictionary<string, object>>(
                  "PlaylistTrack",
                  j => j.HasOne<Track>().WithMany().HasForeignKey("TrackId"),
                  j => j.HasOne<PlayList>().WithMany().HasForeignKey("PlayListId"),
                  j =>
                  {
                      j.HasKey("PlayListId", "TrackId");
                  });

            // Налаштування Many-to-Many для Album ↔ Genre, Artist ↔ Genre, Track ↔ Genre
            modelBuilder.Entity<Album>()
                .HasMany(a => a.Genres)
                .WithMany(g => g.Albums)
                .UsingEntity("AlbumGenre");

            modelBuilder.Entity<Artist>()
                .HasMany(a => a.Genres)
                .WithMany(g => g.Artists)
                .UsingEntity("ArtistGenre");

            modelBuilder.Entity<Track>()
                .HasMany(t => t.Genres)
                .WithMany(g => g.Tracks)
                .UsingEntity("TrackGenre");

            // Налаштування обмежень
            modelBuilder.Entity<RatingAndReview>()
                .Property(r => r.Rating)
                .IsRequired()
                .HasDefaultValue(1)
                .HasAnnotation("CheckConstraint", "Rating >= 1 AND Rating <= 5");

            modelBuilder.Entity<UserCollection>()
                .Property(uc => uc.Status)
                .HasDefaultValue("wanted");

            modelBuilder.Entity<Album>()
                .HasIndex(a => a.ReleaseDate);

            // Додавання початкових даних

            // Жанри
            modelBuilder.Entity<Genre>().HasData(
                new Genre { GenreId = 1, GenreName = "Alternative Metal" },
                new Genre { GenreId = 2, GenreName = "Heavy Metal" },
                new Genre { GenreId = 3, GenreName = "Nu Metal" }
            );

            // Виконавці
            modelBuilder.Entity<Artist>().HasData(
                new Artist
                {
                    ArtistId = 1,
                    bandName = "Three Days Grace",
                    Country = "Canada",
                    yearsOfActivity = "1997-present",
                    Biography = "Three Days Grace is a Canadian rock band formed in Norwood, Ontario in 1997."
                },
                new Artist
                {
                    ArtistId = 2,
                    bandName = "Metallica",
                    Country = "USA",
                    yearsOfActivity = "1981-present",
                    Biography = "Metallica is an American heavy metal band formed in Los Angeles in 1981."
                },
                new Artist
                {
                    ArtistId = 3,
                    bandName = "Disturbed",
                    Country = "USA",
                    yearsOfActivity = "1994-present",
                    Biography = "Disturbed is an American heavy metal band from Chicago, formed in 1994."
                }
            );

            // Альбоми
            modelBuilder.Entity<Album>().HasData(
                // Three Days Grace
                new Album
                {
                    AlbumId = 1,
                    AlbumName = "Three Days Grace",
                    AlbumDuration = 2640, // Приблизно 44 хвилини
                    TrackCount = 12,
                    Label = "Jive Records",
                    Format = "CD",
                    ReleaseDate = new DateTime(2003, 7, 22),
                    ArtistId = 1
                },
                new Album
                {
                    AlbumId = 2,
                    AlbumName = "One-X",
                    AlbumDuration = 2880, // Приблизно 48 хвилин
                    TrackCount = 12,
                    Label = "Jive Records",
                    Format = "CD",
                    ReleaseDate = new DateTime(2006, 6, 13),
                    ArtistId = 1
                },
                // Metallica
                new Album
                {
                    AlbumId = 3,
                    AlbumName = "Kill 'Em All",
                    AlbumDuration = 3060, // Приблизно 51 хвилина
                    TrackCount = 10,
                    Label = "Megaforce Records",
                    Format = "Vinyl",
                    ReleaseDate = new DateTime(1983, 7, 25),
                    ArtistId = 2
                },
                // Disturbed
                new Album
                {
                    AlbumId = 4,
                    AlbumName = "The Sickness",
                    AlbumDuration = 2820, // Приблизно 47 хвилин
                    TrackCount = 12,
                    Label = "Giant Records",
                    Format = "CD",
                    ReleaseDate = new DateTime(2000, 3, 7),
                    ArtistId = 3
                },
                new Album
                {
                    AlbumId = 5,
                    AlbumName = "Ten Thousand Fists",
                    AlbumDuration = 3360, // Приблизно 56 хвилин
                    TrackCount = 14,
                    Label = "Reprise Records",
                    Format = "CD",
                    ReleaseDate = new DateTime(2005, 9, 20),
                    ArtistId = 3
                },
                new Album
                {
                    AlbumId = 6,
                    AlbumName = "Indestructible",
                    AlbumDuration = 2940, // Приблизно 49 хвилин
                    TrackCount = 12,
                    Label = "Reprise Records",
                    Format = "CD",
                    ReleaseDate = new DateTime(2008, 6, 3),
                    ArtistId = 3
                }
            );

            // Треки
            modelBuilder.Entity<Track>().HasData(
                // Three Days Grace - Three Days Grace
                new Track { TrackId = 1, TrackName = "Burn", Duration = 178, NumberInList = 1, LyricsAuthor = "Adam Gontier", MusicAuthor = "Three Days Grace", AlbumId = 1 },
                new Track { TrackId = 2, TrackName = "I Hate Everything About You", Duration = 234, NumberInList = 3, LyricsAuthor = "Adam Gontier", MusicAuthor = "Three Days Grace", AlbumId = 1 },
                // Three Days Grace - One-X
                new Track { TrackId = 3, TrackName = "Animal I Have Become", Duration = 231, NumberInList = 2, LyricsAuthor = "Adam Gontier", MusicAuthor = "Three Days Grace", AlbumId = 2 },
                new Track { TrackId = 4, TrackName = "Pain", Duration = 209, NumberInList = 4, LyricsAuthor = "Adam Gontier", MusicAuthor = "Three Days Grace", AlbumId = 2 },
                // Metallica - Kill 'Em All
                new Track { TrackId = 5, TrackName = "Hit the Lights", Duration = 257, NumberInList = 1, LyricsAuthor = "James Hetfield", MusicAuthor = "Metallica", AlbumId = 3 },
                new Track { TrackId = 6, TrackName = "Seek & Destroy", Duration = 415, NumberInList = 9, LyricsAuthor = "James Hetfield", MusicAuthor = "Metallica", AlbumId = 3 },
                // Disturbed - The Sickness
                new Track { TrackId = 7, TrackName = "Down with the Sickness", Duration = 278, NumberInList = 4, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 4 },
                new Track { TrackId = 8, TrackName = "Stupify", Duration = 227, NumberInList = 2, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 4 },
                // Disturbed - Ten Thousand Fists
                new Track { TrackId = 9, TrackName = "Ten Thousand Fists", Duration = 208, NumberInList = 2, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 5 },
                new Track { TrackId = 10, TrackName = "Stricken", Duration = 243, NumberInList = 4, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 5 },
                // Disturbed - Indestructible
                new Track { TrackId = 11, TrackName = "Indestructible", Duration = 276, NumberInList = 1, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 6 },
                new Track { TrackId = 12, TrackName = "Inside the Fire", Duration = 233, NumberInList = 2, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 6 }
            );



            // Плейлист
            modelBuilder.Entity<PlayList>().HasData(
                new PlayList
                {
                    PlayListId = 1,
                    PlayListName = "Metal Favorites",
                    DateCreated = new DateTime(2025, 4, 13),
                    UserId = 1
                }
            );

            // Проміжна таблиця PlaylistTrack (з порядком треків)
            modelBuilder.Entity("PlaylistTrack").HasData(
                new { PlayListId = 1, TrackId = 3}, // "Animal I Have Become"
                new { PlayListId = 1, TrackId = 6}, // "Seek & Destroy"
                new { PlayListId = 1, TrackId = 7}  // "Down with the Sickness"
            );

            // Рейтинги та відгуки
            modelBuilder.Entity<RatingAndReview>().HasData(
                new RatingAndReview
                {
                    RatingAndReviewId = 1,
                    Rating = 5,
                    Comment = "Amazing debut album!",
                    UserId = 1,
                    AlbumId = 1 // Three Days Grace - Three Days Grace
                },
                new RatingAndReview
                {
                    RatingAndReviewId = 2,
                    Rating = 4,
                    Comment = "Classic thrash metal album.",
                    UserId = 1,
                    AlbumId = 3 // Metallica - Kill 'Em All
                },
                new RatingAndReview
                {
                    RatingAndReviewId = 3,
                    Rating = 5,
                    Comment = "One of my favorites!",
                    UserId = 1,
                    AlbumId = 4 // Disturbed - The Sickness
                }
            );

            // Колекція користувача
            modelBuilder.Entity<UserCollection>().HasData(
                new UserCollection
                {
                    UserCollectionId = 1,
                    DateAdded = new DateTime(2025, 4, 1),
                    Status = "bought",
                    AlbumId = 1, // Three Days Grace - Three Days Grace
                    UserId = 1
                },
                new UserCollection
                {
                    UserCollectionId = 2,
                    DateAdded = new DateTime(2025, 4, 2),
                    Status = "wanted",
                    AlbumId = 3, // Metallica - Kill 'Em All
                    UserId = 1
                }
            );

            // Зв’язки Artist-Genre
            modelBuilder.Entity("ArtistGenre").HasData(
                new { ArtistsArtistId = 1, GenresGenreId = 1 }, // Three Days Grace - Alternative Metal
                new { ArtistsArtistId = 2, GenresGenreId = 2 }, // Metallica - Heavy Metal
                new { ArtistsArtistId = 3, GenresGenreId = 3 }  // Disturbed - Nu Metal
            );

            // Зв’язки Album-Genre
            modelBuilder.Entity("AlbumGenre").HasData(
                new { AlbumsAlbumId = 1, GenresGenreId = 1 }, // Three Days Grace - Alternative Metal
                new { AlbumsAlbumId = 2, GenresGenreId = 1 }, // One-X - Alternative Metal
                new { AlbumsAlbumId = 3, GenresGenreId = 2 }, // Kill 'Em All - Heavy Metal
                new { AlbumsAlbumId = 4, GenresGenreId = 3 }, // The Sickness - Nu Metal
                new { AlbumsAlbumId = 5, GenresGenreId = 3 }, // Ten Thousand Fists - Nu Metal
                new { AlbumsAlbumId = 6, GenresGenreId = 3 }  // Indestructible - Nu Metal
            );

            // Зв’язки Track-Genre
            modelBuilder.Entity("TrackGenre").HasData(
                new { TracksTrackId = 1, GenresGenreId = 1 }, new { TracksTrackId = 2, GenresGenreId = 1 }, // Three Days Grace tracks
                new { TracksTrackId = 3, GenresGenreId = 1 }, new { TracksTrackId = 4, GenresGenreId = 1 },
                new { TracksTrackId = 5, GenresGenreId = 2 }, new { TracksTrackId = 6, GenresGenreId = 2 }, // Metallica tracks
                new { TracksTrackId = 7, GenresGenreId = 3 }, new { TracksTrackId = 8, GenresGenreId = 3 }, // Disturbed tracks
                new { TracksTrackId = 9, GenresGenreId = 3 }, new { TracksTrackId = 10, GenresGenreId = 3 },
                new { TracksTrackId = 11, GenresGenreId = 3 }, new { TracksTrackId = 12, GenresGenreId = 3 }
            );
        }

    }
}
