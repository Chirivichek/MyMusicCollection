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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use Fluent API configurations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //Ganres
            modelBuilder.Entity<Genre>().HasData(
                new Genre { GenreId = 1, GenreName = "Post-Grunge" },
                new Genre { GenreId = 2, GenreName = "Alternative Metal" },
                new Genre { GenreId = 3, GenreName = "Thrash Metal" },
                new Genre { GenreId = 4, GenreName = "Heavy Metal" },
                new Genre { GenreId = 5, GenreName = "Nu Metal" }
            );

            // Artists
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

            // Albums
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

            // Traks
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

            // Users (need for PlayList)
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

            // Playlists
            modelBuilder.Entity<PlayList>().HasData(
                new PlayList
                {
                    PlayListId = 1,
                    PlayListName = "Metal Favorites",
                    DateCreated = new DateTime(2025, 4, 13),
                    UserId = 1
                }
            );

            // Connections PlaylistTrack
            modelBuilder.Entity("PlaylistTrack").HasData(
                new { PlayListId = 1, TrackId = 5 },  // Animal I Have Become (Three Days Grace)
                new { PlayListId = 1, TrackId = 13 }, // Enter Sandman (Metallica)
                new { PlayListId = 1, TrackId = 23 }  // The Sound of Silence (Disturbed)
            );

            // Connections Artist-Genre
            modelBuilder.Entity("ArtistGenre").HasData(
                new { ArtistsArtistId = 1, GenresGenreId = 1 }, // Three Days Grace - Post-Grunge
                new { ArtistsArtistId = 1, GenresGenreId = 2 }, // Three Days Grace - Alternative Metal
                new { ArtistsArtistId = 2, GenresGenreId = 3 }, // Metallica - Thrash Metal
                new { ArtistsArtistId = 2, GenresGenreId = 4 }, // Metallica - Heavy Metal
                new { ArtistsArtistId = 3, GenresGenreId = 4 }, // Disturbed - Heavy Metal
                new { ArtistsArtistId = 3, GenresGenreId = 5 }  // Disturbed - Nu Metal
            );
        }
        }
    }
