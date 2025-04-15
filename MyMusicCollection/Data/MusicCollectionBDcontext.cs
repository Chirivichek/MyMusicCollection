﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyMusicCollection.Entitys;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace MyMusicCollection.Data
{
    public class MusicCollectionBDcontext : DbContext
    {
        public MusicCollectionBDcontext(DbContextOptions options) : base(options)
        {
        }

        // entities : album, track, artist, userCollection, ratingsAndReviews, Playlists

        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<RatingAndReview> RatingsAndReviews { get; set; }
        public DbSet<PlayList> PlayLists { get; set; }
        public DbSet<UserCollection> UserCollections { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<User> Users { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                var cs = ConfigurationManager.ConnectionStrings["MusicCollectionDb"].ConnectionString;
                optionsBuilder.UseSqlServer(cs);
            }
        }
        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // Налаштування Many-to-Many для Playlist ↔ Track
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
                    AlbumDuration = 178 + 234 + 186 + 210 + 195, // Сума тривалостей треків
                    TrackCount = 5,
                    Label = "Jive Records",
                    Format = "CD",
                    ReleaseDate = new DateTime(2003, 7, 22),
                    ArtistId = 1
                },
                new Album
                {
                    AlbumId = 2,
                    AlbumName = "One-X",
                    AlbumDuration = 231 + 209 + 204 + 218 + 190, // Сума тривалостей треків
                    TrackCount = 5,
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
                    AlbumDuration = 257 + 415 + 306 + 188 + 280, // Сума тривалостей треків
                    TrackCount = 5,
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
                    AlbumDuration = 278 + 227 + 201 + 235 + 192, // Сума тривалостей треків
                    TrackCount = 5,
                    Label = "Giant Records",
                    Format = "CD",
                    ReleaseDate = new DateTime(2000, 3, 7),
                    ArtistId = 3
                },
                new Album
                {
                    AlbumId = 5,
                    AlbumName = "Ten Thousand Fists",
                    AlbumDuration = 208 + 243 + 223 + 200 + 215, // Сума тривалостей треків
                    TrackCount = 5,
                    Label = "Reprise Records",
                    Format = "CD",
                    ReleaseDate = new DateTime(2005, 9, 20),
                    ArtistId = 3
                },
                new Album
                {
                    AlbumId = 6,
                    AlbumName = "Indestructible",
                    AlbumDuration = 276 + 233 + 248 + 206 + 220, // Сума тривалостей треків
                    TrackCount = 5,
                    Label = "Reprise Records",
                    Format = "CD",
                    ReleaseDate = new DateTime(2008, 6, 3),
                    ArtistId = 3
                }
            );

            // Треки
            modelBuilder.Entity<Track>().HasData(
                // Three Days Grace - Three Days Grace (AlbumId = 1)
                new Track { TrackId = 1, TrackName = "Burn", Duration = 178, NumberInList = 1, LyricsAuthor = "Adam Gontier", MusicAuthor = "Three Days Grace", AlbumId = 1 },
                new Track { TrackId = 2, TrackName = "I Hate Everything About You", Duration = 234, NumberInList = 2, LyricsAuthor = "Adam Gontier", MusicAuthor = "Three Days Grace", AlbumId = 1 },
                new Track { TrackId = 3, TrackName = "Just Like You", Duration = 186, NumberInList = 3, LyricsAuthor = "Adam Gontier", MusicAuthor = "Three Days Grace", AlbumId = 1 },
                new Track { TrackId = 4, TrackName = "Home", Duration = 210, NumberInList = 4, LyricsAuthor = "Adam Gontier", MusicAuthor = "Three Days Grace", AlbumId = 1 },
                new Track { TrackId = 5, TrackName = "Scared", Duration = 195, NumberInList = 5, LyricsAuthor = "Adam Gontier", MusicAuthor = "Three Days Grace", AlbumId = 1 },

                // Three Days Grace - One-X (AlbumId = 2)
                new Track { TrackId = 6, TrackName = "Animal I Have Become", Duration = 231, NumberInList = 1, LyricsAuthor = "Adam Gontier", MusicAuthor = "Three Days Grace", AlbumId = 2 },
                new Track { TrackId = 7, TrackName = "Pain", Duration = 209, NumberInList = 2, LyricsAuthor = "Adam Gontier", MusicAuthor = "Three Days Grace", AlbumId = 2 },
                new Track { TrackId = 8, TrackName = "Never Too Late", Duration = 204, NumberInList = 3, LyricsAuthor = "Adam Gontier", MusicAuthor = "Three Days Grace", AlbumId = 2 },
                new Track { TrackId = 9, TrackName = "Riot", Duration = 218, NumberInList = 4, LyricsAuthor = "Adam Gontier", MusicAuthor = "Three Days Grace", AlbumId = 2 },
                new Track { TrackId = 10, TrackName = "On My Own", Duration = 190, NumberInList = 5, LyricsAuthor = "Adam Gontier", MusicAuthor = "Three Days Grace", AlbumId = 2 },

                // Metallica - Kill 'Em All (AlbumId = 3)
                new Track { TrackId = 11, TrackName = "Hit the Lights", Duration = 257, NumberInList = 1, LyricsAuthor = "James Hetfield", MusicAuthor = "Metallica", AlbumId = 3 },
                new Track { TrackId = 12, TrackName = "The Four Horsemen", Duration = 306, NumberInList = 2, LyricsAuthor = "James Hetfield", MusicAuthor = "Metallica", AlbumId = 3 },
                new Track { TrackId = 13, TrackName = "Motorbreath", Duration = 188, NumberInList = 3, LyricsAuthor = "James Hetfield", MusicAuthor = "Metallica", AlbumId = 3 },
                new Track { TrackId = 14, TrackName = "Jump in the Fire", Duration = 280, NumberInList = 4, LyricsAuthor = "James Hetfield", MusicAuthor = "Metallica", AlbumId = 3 },
                new Track { TrackId = 15, TrackName = "Seek & Destroy", Duration = 415, NumberInList = 5, LyricsAuthor = "James Hetfield", MusicAuthor = "Metallica", AlbumId = 3 },

                // Disturbed - The Sickness (AlbumId = 4)
                new Track { TrackId = 16, TrackName = "Voices", Duration = 201, NumberInList = 1, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 4 },
                new Track { TrackId = 17, TrackName = "Stupify", Duration = 227, NumberInList = 2, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 4 },
                new Track { TrackId = 18, TrackName = "The Game", Duration = 235, NumberInList = 3, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 4 },
                new Track { TrackId = 19, TrackName = "Down with the Sickness", Duration = 278, NumberInList = 4, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 4 },
                new Track { TrackId = 20, TrackName = "Fear", Duration = 192, NumberInList = 5, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 4 },

                // Disturbed - Ten Thousand Fists (AlbumId = 5)
                new Track { TrackId = 21, TrackName = "Guarded", Duration = 200, NumberInList = 1, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 5 },
                new Track { TrackId = 22, TrackName = "Ten Thousand Fists", Duration = 208, NumberInList = 2, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 5 },
                new Track { TrackId = 23, TrackName = "Just Stop", Duration = 223, NumberInList = 3, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 5 },
                new Track { TrackId = 24, TrackName = "Stricken", Duration = 243, NumberInList = 4, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 5 },
                new Track { TrackId = 25, TrackName = "Deify", Duration = 215, NumberInList = 5, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 5 },

                // Disturbed - Indestructible (AlbumId = 6)
                new Track { TrackId = 26, TrackName = "Indestructible", Duration = 276, NumberInList = 1, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 6 },
                new Track { TrackId = 27, TrackName = "Inside the Fire", Duration = 233, NumberInList = 2, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 6 },
                new Track { TrackId = 28, TrackName = "The Night", Duration = 248, NumberInList = 3, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 6 },
                new Track { TrackId = 29, TrackName = "Perfect Insanity", Duration = 206, NumberInList = 4, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 6 },
                new Track { TrackId = 30, TrackName = "Haunted", Duration = 220, NumberInList = 5, LyricsAuthor = "David Draiman", MusicAuthor = "Disturbed", AlbumId = 6 }
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

            // Проміжна таблиця PlaylistTrack
            modelBuilder.Entity("PlaylistTrack").HasData(
                new { PlayListId = 1, TrackId = 6 },  // Animal I Have Become
                new { PlayListId = 1, TrackId = 15 }, // Seek & Destroy
                new { PlayListId = 1, TrackId = 19 }  // Down with the Sickness
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
                // Three Days Grace tracks (Alternative Metal, GenreId = 1)
                new { TracksTrackId = 1, GenresGenreId = 1 },
                new { TracksTrackId = 2, GenresGenreId = 1 },
                new { TracksTrackId = 3, GenresGenreId = 1 },
                new { TracksTrackId = 4, GenresGenreId = 1 },
                new { TracksTrackId = 5, GenresGenreId = 1 },
                new { TracksTrackId = 6, GenresGenreId = 1 },
                new { TracksTrackId = 7, GenresGenreId = 1 },
                new { TracksTrackId = 8, GenresGenreId = 1 },
                new { TracksTrackId = 9, GenresGenreId = 1 },
                new { TracksTrackId = 10, GenresGenreId = 1 },

                // Metallica tracks (Heavy Metal, GenreId = 2)
                new { TracksTrackId = 11, GenresGenreId = 2 },
                new { TracksTrackId = 12, GenresGenreId = 2 },
                new { TracksTrackId = 13, GenresGenreId = 2 },
                new { TracksTrackId = 14, GenresGenreId = 2 },
                new { TracksTrackId = 15, GenresGenreId = 2 },

                // Disturbed tracks (Nu Metal, GenreId = 3)
                new { TracksTrackId = 16, GenresGenreId = 3 },
                new { TracksTrackId = 17, GenresGenreId = 3 },
                new { TracksTrackId = 18, GenresGenreId = 3 },
                new { TracksTrackId = 19, GenresGenreId = 3 },
                new { TracksTrackId = 20, GenresGenreId = 3 },
                new { TracksTrackId = 21, GenresGenreId = 3 },
                new { TracksTrackId = 22, GenresGenreId = 3 },
                new { TracksTrackId = 23, GenresGenreId = 3 },
                new { TracksTrackId = 24, GenresGenreId = 3 },
                new { TracksTrackId = 25, GenresGenreId = 3 },
                new { TracksTrackId = 26, GenresGenreId = 3 },
                new { TracksTrackId = 27, GenresGenreId = 3 },
                new { TracksTrackId = 28, GenresGenreId = 3 },
                new { TracksTrackId = 29, GenresGenreId = 3 },
                new { TracksTrackId = 30, GenresGenreId = 3 }
            );
        }
        */


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Застосовуємо Fluent API конфігурації
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Жанри
            modelBuilder.Entity<Genre>().HasData(
                new Genre { GenreId = 1, GenreName = "Post-Grunge" },
                new Genre { GenreId = 2, GenreName = "Alternative Metal" },
                new Genre { GenreId = 3, GenreName = "Thrash Metal" },
                new Genre { GenreId = 4, GenreName = "Heavy Metal" },
                new Genre { GenreId = 5, GenreName = "Nu Metal" }
            );

            // Виконавці
            modelBuilder.Entity<Artist>().HasData(
                new Artist
                {
                    ArtistId = 1,
                    bandName = "Three Days Grace",
                    Country = "Canada",
                    yearsOfActivity = "1997-present",
                    Biography = "Canadian rock band formed in Norwood, Ontario."
                },
                new Artist
                {
                    ArtistId = 2,
                    bandName = "Metallica",
                    Country = "USA",
                    yearsOfActivity = "1981-present",
                    Biography = "American heavy metal band, one of the 'Big Four' of thrash metal."
                },
                new Artist
                {
                    ArtistId = 3,
                    bandName = "Disturbed",
                    Country = "USA",
                    yearsOfActivity = "1994-present",
                    Biography = "American heavy metal band from Chicago."
                }
            );

            // Альбоми
            modelBuilder.Entity<Album>().HasData(
                // Three Days Grace
                new Album
                {
                    AlbumId = 1,
                    AlbumName = "Three Days Grace",
                    AlbumDuration = 934, // 258+192+229+255
                    TrackCount = 4,
                    Label = "Jive Records",
                    Format = "CD",
                    ReleaseDate = new DateTime(2003, 7, 22),
                    ArtistId = 1
                },
                new Album
                {
                    AlbumId = 2,
                    AlbumName = "One-X",
                    AlbumDuration = 855, // 231+209+208+207
                    TrackCount = 4,
                    Label = "Jive Records",
                    Format = "CD",
                    ReleaseDate = new DateTime(2006, 6, 13),
                    ArtistId = 1
                },
                // Metallica
                new Album
                {
                    AlbumId = 3,
                    AlbumName = "Master of Puppets",
                    AlbumDuration = 1710, // 312+515+387+496
                    TrackCount = 4,
                    Label = "Elektra Records",
                    Format = "Vinyl",
                    ReleaseDate = new DateTime(1986, 3, 3),
                    ArtistId = 2
                },
                new Album
                {
                    AlbumId = 4,
                    AlbumName = "Metallica",
                    AlbumDuration = 1429, // 331+324+386+388
                    TrackCount = 4,
                    Label = "Elektra Records",
                    Format = "CD",
                    ReleaseDate = new DateTime(1991, 8, 12),
                    ArtistId = 2
                },
                // Disturbed
                new Album
                {
                    AlbumId = 5,
                    AlbumName = "Ten Thousand Fists",
                    AlbumDuration = 1029, // 214+245+283+287
                    TrackCount = 4,
                    Label = "Reprise Records",
                    Format = "CD",
                    ReleaseDate = new DateTime(2005, 9, 20),
                    ArtistId = 3
                },
                new Album
                {
                    AlbumId = 6,
                    AlbumName = "Immortalized",
                    AlbumDuration = 988, // 253+263+245+227
                    TrackCount = 4,
                    Label = "Reprise Records",
                    Format = "Digital",
                    ReleaseDate = new DateTime(2015, 8, 21),
                    ArtistId = 3
                }
            );

            // Треки
            modelBuilder.Entity<Track>().HasData(
                // Three Days Grace - Three Days Grace
                new Track
                {
                    TrackId = 1,
                    TrackName = "Burn",
                    Duration = 258, // 4:18
                    NumberInList = 1,
                    LyricsAuthor = "Adam Gontier",
                    MusicAuthor = "Three Days Grace",
                    AlbumId = 1
                },
                new Track
                {
                    TrackId = 2,
                    TrackName = "Just Like You",
                    Duration = 192, // 3:12
                    NumberInList = 2,
                    LyricsAuthor = "Adam Gontier",
                    MusicAuthor = "Three Days Grace",
                    AlbumId = 1
                },
                new Track
                {
                    TrackId = 3,
                    TrackName = "I Hate Everything About You",
                    Duration = 229, // 3:49
                    NumberInList = 3,
                    LyricsAuthor = "Adam Gontier",
                    MusicAuthor = "Three Days Grace",
                    AlbumId = 1
                },
                new Track
                {
                    TrackId = 4,
                    TrackName = "Home",
                    Duration = 255, // 4:15
                    NumberInList = 4,
                    LyricsAuthor = "Adam Gontier",
                    MusicAuthor = "Three Days Grace",
                    AlbumId = 1
                },
                // Three Days Grace - One-X
                new Track
                {
                    TrackId = 5,
                    TrackName = "Animal I Have Become",
                    Duration = 231, // 3:51
                    NumberInList = 1,
                    LyricsAuthor = "Adam Gontier",
                    MusicAuthor = "Three Days Grace",
                    AlbumId = 2
                },
                new Track
                {
                    TrackId = 6,
                    TrackName = "Pain",
                    Duration = 209, // 3:29
                    NumberInList = 2,
                    LyricsAuthor = "Adam Gontier",
                    MusicAuthor = "Three Days Grace",
                    AlbumId = 2
                },
                new Track
                {
                    TrackId = 7,
                    TrackName = "Never Too Late",
                    Duration = 208, // 3:28
                    NumberInList = 3,
                    LyricsAuthor = "Adam Gontier",
                    MusicAuthor = "Three Days Grace",
                    AlbumId = 2
                },
                new Track
                {
                    TrackId = 8,
                    TrackName = "Riot",
                    Duration = 207, // 3:27
                    NumberInList = 4,
                    LyricsAuthor = "Adam Gontier",
                    MusicAuthor = "Three Days Grace",
                    AlbumId = 2
                },
                // Metallica - Master of Puppets
                new Track
                {
                    TrackId = 9,
                    TrackName = "Battery",
                    Duration = 312, // 5:12
                    NumberInList = 1,
                    LyricsAuthor = "James Hetfield",
                    MusicAuthor = "Metallica",
                    AlbumId = 3
                },
                new Track
                {
                    TrackId = 10,
                    TrackName = "Master of Puppets",
                    Duration = 515, // 8:35
                    NumberInList = 2,
                    LyricsAuthor = "James Hetfield",
                    MusicAuthor = "Metallica",
                    AlbumId = 3
                },
                new Track
                {
                    TrackId = 11,
                    TrackName = "Welcome Home (Sanitarium)",
                    Duration = 387, // 6:27
                    NumberInList = 3,
                    LyricsAuthor = "James Hetfield",
                    MusicAuthor = "Metallica",
                    AlbumId = 3
                },
                new Track
                {
                    TrackId = 12,
                    TrackName = "Disposable Heroes",
                    Duration = 496, // 8:16
                    NumberInList = 4,
                    LyricsAuthor = "James Hetfield",
                    MusicAuthor = "Metallica",
                    AlbumId = 3
                },
                // Metallica - Metallica (The Black Album)
                new Track
                {
                    TrackId = 13,
                    TrackName = "Enter Sandman",
                    Duration = 331, // 5:31
                    NumberInList = 1,
                    LyricsAuthor = "James Hetfield",
                    MusicAuthor = "Metallica",
                    AlbumId = 4
                },
                new Track
                {
                    TrackId = 14,
                    TrackName = "Sad But True",
                    Duration = 324, // 5:24
                    NumberInList = 2,
                    LyricsAuthor = "James Hetfield",
                    MusicAuthor = "Metallica",
                    AlbumId = 4
                },
                new Track
                {
                    TrackId = 15,
                    TrackName = "The Unforgiven",
                    Duration = 386, // 6:26
                    NumberInList = 3,
                    LyricsAuthor = "James Hetfield",
                    MusicAuthor = "Metallica",
                    AlbumId = 4
                },
                new Track
                {
                    TrackId = 16,
                    TrackName = "Nothing Else Matters",
                    Duration = 388, // 6:28
                    NumberInList = 4,
                    LyricsAuthor = "James Hetfield",
                    MusicAuthor = "Metallica",
                    AlbumId = 4
                },
                // Disturbed - Ten Thousand Fists
                new Track
                {
                    TrackId = 17,
                    TrackName = "Ten Thousand Fists",
                    Duration = 214, // 3:34
                    NumberInList = 1,
                    LyricsAuthor = "David Draiman",
                    MusicAuthor = "Disturbed",
                    AlbumId = 5
                },
                new Track
                {
                    TrackId = 18,
                    TrackName = "Stricken",
                    Duration = 245, // 4:05
                    NumberInList = 2,
                    LyricsAuthor = "David Draiman",
                    MusicAuthor = "Disturbed",
                    AlbumId = 5
                },
                new Track
                {
                    TrackId = 19,
                    TrackName = "I'm Alive",
                    Duration = 283, // 4:43
                    NumberInList = 3,
                    LyricsAuthor = "David Draiman",
                    MusicAuthor = "Disturbed",
                    AlbumId = 5
                },
                new Track
                {
                    TrackId = 20,
                    TrackName = "Land of Confusion",
                    Duration = 287, // 4:47
                    NumberInList = 4,
                    LyricsAuthor = "David Draiman",
                    MusicAuthor = "Disturbed",
                    AlbumId = 5
                },
                // Disturbed - Immortalized
                new Track
                {
                    TrackId = 21,
                    TrackName = "The Vengeful One",
                    Duration = 253, // 4:13
                    NumberInList = 1,
                    LyricsAuthor = "David Draiman",
                    MusicAuthor = "Disturbed",
                    AlbumId = 6
                },
                new Track
                {
                    TrackId = 22,
                    TrackName = "Immortalized",
                    Duration = 263, // 4:23
                    NumberInList = 2,
                    LyricsAuthor = "David Draiman",
                    MusicAuthor = "Disturbed",
                    AlbumId = 6
                },
                new Track
                {
                    TrackId = 23,
                    TrackName = "The Sound of Silence",
                    Duration = 245, // 4:05
                    NumberInList = 3,
                    LyricsAuthor = "David Draiman",
                    MusicAuthor = "Disturbed",
                    AlbumId = 6
                },
                new Track
                {
                    TrackId = 24,
                    TrackName = "Fire It Up",
                    Duration = 227, // 3:47
                    NumberInList = 4,
                    LyricsAuthor = "David Draiman",
                    MusicAuthor = "Disturbed",
                    AlbumId = 6
                }
            );

            // Користувачі (потрібно для PlayList)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    UserName = "MusicFan",
                    Email = "musicfan@example.com",
                    Password = "SecurePass123",
                    DateOfBirth = new DateTime(1990, 5, 15)
                }
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

            // Зв’язки PlaylistTrack
            modelBuilder.Entity("PlaylistTrack").HasData(
                new { PlayListId = 1, TrackId = 5 },  // Animal I Have Become (Three Days Grace)
                new { PlayListId = 1, TrackId = 13 }, // Enter Sandman (Metallica)
                new { PlayListId = 1, TrackId = 23 }  // The Sound of Silence (Disturbed)
            );

            // Зв’язки Artist-Genre
            modelBuilder.Entity("ArtistGenre").HasData(
                new { ArtistsArtistId = 1, GenresGenreId = 1 }, // Three Days Grace - Post-Grunge
                new { ArtistsArtistId = 1, GenresGenreId = 2 }, // Three Days Grace - Alternative Metal
                new { ArtistsArtistId = 2, GenresGenreId = 3 }, // Metallica - Thrash Metal
                new { ArtistsArtistId = 2, GenresGenreId = 4 }, // Metallica - Heavy Metal
                new { ArtistsArtistId = 3, GenresGenreId = 4 }, // Disturbed - Heavy Metal
                new { ArtistsArtistId = 3, GenresGenreId = 5 }  // Disturbed - Nu Metal
            );

            // Зв’язки Album-Genre

        }
        }
    }
