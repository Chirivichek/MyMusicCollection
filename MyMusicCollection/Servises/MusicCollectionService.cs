using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyMusicCollection.Data;
using MyMusicCollection.Entitys;
using MyMusicCollection.Interface;

namespace MyMusicCollection.Services
{
    public class MusicCollectionService : IMusicCollectionService
    {
        private readonly MusicCollectionBDcontext _context;

        public MusicCollectionService(MusicCollectionBDcontext context)
        {
            _context = context;
        }
        public void ShowUserInfo(User currentUser) // Method to show user info
        {
            if (currentUser == null)
            {
                Console.WriteLine("User is null.");
                return;
            }
            Console.WriteLine("--------------- User Info ----------------");
            Console.WriteLine($"User ID: {currentUser.UserId}");
            Console.WriteLine($"Username: {currentUser.UserName}");
            Console.WriteLine($"Email: {currentUser.Email}");
            Console.WriteLine($"Date of Birth: {currentUser.DateOfBirth.ToShortDateString()}");
            Console.WriteLine("------------------------------------------");
        }
        public void ViewPlaylists(User currentUser) // Method to view user playlists
        {
            if (currentUser?.PlayLists == null || !currentUser.PlayLists.Any())
            {
                Console.WriteLine("No playlists available.");
                return;
            }

            Console.WriteLine("Select a playlist to view:");
            int index = 1;
            foreach (var playlist in currentUser.PlayLists)
            {
                Console.WriteLine($"[{index}] {playlist.PlayListName}");
                index++;
            }
            // Get user input for playlist selection
            if (!int.TryParse(Console.ReadLine(), out int playlistIndex) || playlistIndex < 1 || playlistIndex > currentUser.PlayLists.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }
            // Get the selected playlist
            var selectedPlaylist = currentUser.PlayLists.ElementAt(playlistIndex - 1);
            var allTracks = _context.PlayLists
             .Include(p => p.Tracks)
             .ThenInclude(t => t.Album)
             .ThenInclude(a => a.Artist)
             .FirstOrDefault(p => p.PlayListId == currentUser.PlayLists.ElementAt(playlistIndex - 1).PlayListId);
            if (allTracks == null)
            {
                Console.WriteLine("Playlist not found in database.");
                return;
            }
            if (selectedPlaylist == null)
            {
                Console.WriteLine("Playlist not found in database.");
                return;
            }

            if (selectedPlaylist.Tracks == null || !selectedPlaylist.Tracks.Any())
            {
                Console.WriteLine("No tracks in this playlist.");
                return;
            }

            Console.WriteLine($"Tracks in playlist '{selectedPlaylist.PlayListName}':");
            Console.WriteLine("Sort by:");
            Console.WriteLine("[1] Track name");
            Console.WriteLine("[2] Album name");
            Console.WriteLine("[3] Artist");
            Console.WriteLine("Enter your choice (1-3, or press Enter for default):");

            string sortChoice = Console.ReadLine();
            IEnumerable<Track> sortedTracks = allTracks.Tracks;
            switch (sortChoice)
            {
                case "1":
                    sortedTracks = allTracks.Tracks.OrderBy(t => t.TrackName);
                    break;
                case "2":
                    sortedTracks = allTracks.Tracks.OrderBy(t => t.Album.AlbumName);
                    break;
                case "3":
                    sortedTracks = allTracks.Tracks.OrderBy(t => t.Album.Artist.bandName);
                    break;
                default:
                    // Default sorting (no sorting)
                    break;
            }
            index = 1;
            foreach (var track in allTracks.Tracks)
            {
                Console.WriteLine($"[{index}] {track.TrackName} - {track.Album.AlbumName} ({track.Album.Artist.bandName})");
                index++;
            }
            Console.WriteLine();
        }

        public void CreatePlaylist(User currentUser) // Method to create a new playlist
        {
            if (currentUser == null)
            {
                Console.WriteLine("User is null.");
                return;
            }
            Console.WriteLine("Enter the name of the new playlist:");
            string playlistName = Console.ReadLine();
            PlayList newPlaylist = new PlayList
            {
                PlayListName = playlistName,
                DateCreated = DateTime.Now,
                UserId = currentUser.UserId,
                Tracks = new List<Track>() // Initialize the Tracks collection

            };
            if (currentUser.PlayLists == null)
            {
                currentUser.PlayLists = new List<PlayList>();
            }
            currentUser.PlayLists.Add(newPlaylist);
            _context.PlayLists.Add(newPlaylist);
            _context.SaveChanges();
            Console.WriteLine($"Playlist '{playlistName}' created successfully.");
        }
        public void AddTrackToPlaylist(User currentUser) // Method to add a track to a playlist
        {
            // Convert to null for the User
            if (currentUser == null)
            {
                Console.WriteLine("User is null.");
                return;
            }
            // Check if the user has any playlists
            if (currentUser.PlayLists == null || !currentUser.PlayLists.Any())
            {
                Console.WriteLine("No playlists available.");
                return;
            }
            Console.WriteLine("Select a playlist to add a track to:");
            int index = 1;
            foreach (var playlist in currentUser.PlayLists)
            {
                Console.WriteLine($"[{index}] {playlist.PlayListName}");
                index++;
            }
            // Get user input for playlist selection
            if (!int.TryParse(Console.ReadLine(), out int playlistIndex) || playlistIndex < 1 || playlistIndex > currentUser.PlayLists.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }
            // Get the selected playlist
            var selectedPlaylist = currentUser.PlayLists.ElementAt(playlistIndex - 1);

            var playlistInContext = _context.PlayLists
            .Include(p => p.Tracks)
            .FirstOrDefault(p => p.PlayListId == selectedPlaylist.PlayListId);

            if (playlistInContext == null)
            {
                Console.WriteLine("Playlist not found in database.");
                return;
            }

            var allTracks = _context.Tracks
            .Include(t => t.Album)
            .ThenInclude(a => a.Artist)
            .ToList();

            if (!allTracks.Any())
            {
                Console.WriteLine("No tracks available to add.");
                return;
            }
            // Display all available tracks
            Console.WriteLine("Available Tracks:");
            index = 1;
            foreach (var track in allTracks)
            {
                Console.WriteLine($"[{index}] {track.TrackName} - {track.Album.AlbumName} ({track.Album.Artist.bandName})");
                index++;
            }
            // Get user input for track selection
            Console.WriteLine("Enter the track number to add:");
            if (!int.TryParse(Console.ReadLine(), out int trackIndex) || trackIndex < 1 || trackIndex > allTracks.Count)
            {
                Console.WriteLine("Invalid track selection.");
                return;
            }
            // Get the selected track
            var selectedTrack = allTracks.ElementAt(trackIndex - 1);
            // Check if the track is already in the playlist
            if (playlistInContext.Tracks.Any(t => t.TrackId == selectedTrack.TrackId))
            {
                Console.WriteLine("Track is already in the playlist.");
                return;
            }
            // Add the track to the playlist
            if (playlistInContext.Tracks == null)
            {
                playlistInContext.Tracks = new List<Track>();
            }
            playlistInContext.Tracks.Add(selectedTrack);
            // Save changes to the database
            _context.SaveChanges();


            if (selectedPlaylist.Tracks == null)
            {
                selectedPlaylist.Tracks = new List<Track>();
            }
            if (!selectedPlaylist.Tracks.Any(t => t.TrackId == selectedTrack.TrackId))
            {
                selectedPlaylist.Tracks.Add(selectedTrack);
            }

            Console.WriteLine($"Track '{selectedTrack.TrackName}' added to playlist '{selectedPlaylist.PlayListName}' successfully.");
        }

        public void ClearPlaylistTracks(User currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("User is null.");
                return;
            }
            if (currentUser.PlayLists == null || !currentUser.PlayLists.Any())
            {
                Console.WriteLine("No playlists available.");
                return;
            }

            Console.WriteLine("Select a playlist to clear:");
            int index = 1;
            foreach (var playlist in currentUser.PlayLists)
            {
                Console.WriteLine($"[{index}] {playlist.PlayListName}");
                index++;
            }

            if (!int.TryParse(Console.ReadLine(), out int playlistIndex) || playlistIndex < 1 || playlistIndex > currentUser.PlayLists.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            var selectedPlaylist = currentUser.PlayLists.ElementAt(playlistIndex - 1);

            // Видаляємо всі записи з PlaylistTrack для цього плейлиста
            _context.Database.ExecuteSqlRaw(
                $"DELETE FROM PlaylistTrack WHERE PlayListId = {selectedPlaylist.PlayListId}");

            // Очищаємо Tracks у пам’яті
            selectedPlaylist.Tracks?.Clear();

            Console.WriteLine($"Playlist '{selectedPlaylist.PlayListName}' cleared successfully.");
        }// Method to delete all tracks from a playlist

        public void ViewRatingsAndReviews(User currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("User is null.");
                return;
            }

            Console.WriteLine("What would you like to do?");
            Console.WriteLine("[1] View your ratings and reviews");
            Console.WriteLine("[2] View all ratings and reviews for an album");
            Console.WriteLine("Enter your choice (1 or 2):");

            if (!int.TryParse(Console.ReadLine(), out int choice) || (choice != 1 && choice != 2))
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            if (choice == 1)
            {
                if (currentUser.RatingsAndReviews == null || !currentUser.RatingsAndReviews.Any())
                {
                    Console.WriteLine("You have no ratings and reviews.");
                    return;
                }
                 var userRatingsAndReviews = _context.RatingsAndReviews
                    .Include(r => r.Album)
                    .ThenInclude(a => a.Artist)
                    .Where(r => r.UserId == currentUser.UserId)
                    .ToList();

                Console.WriteLine("Sort by:");
                Console.WriteLine("[1] Rating");
                Console.WriteLine("[2] Album name");
                Console.WriteLine("Enter your choice (1-2, or press Enter for default):");

                string sortChoice = Console.ReadLine();
                IEnumerable<RatingAndReview> sortedRatings = userRatingsAndReviews;


                switch (sortChoice)
                {
                    case "1": 
                        sortedRatings = userRatingsAndReviews.OrderBy(r => r.Rating); 
                        break;
                    case "2":
                        sortedRatings = userRatingsAndReviews.OrderBy(r => r.Album.AlbumName);
                        break;
                    default:
                        // За замовчуванням сортуємо за рейтингом
                        sortedRatings = userRatingsAndReviews.OrderBy(r => r.Rating);
                        break;
                }


                Console.WriteLine("Your ratings and reviews:");
                int index = 1;
                foreach (var ratingAndReview in currentUser.RatingsAndReviews)
                {
                    var album = _context.Albums
                        .Include(a => a.Artist)
                        .FirstOrDefault(a => a.AlbumId == ratingAndReview.AlbumId);

                    if (album != null)
                    {
                        Console.WriteLine($"[{index}] Album: {album.AlbumName}, Artist: {album.Artist.bandName}");
                        Console.WriteLine($"    Rating: {ratingAndReview.Rating}, Comment: {ratingAndReview.Comment ?? "No comment"}");
                    }
                    index++;
                }
            }
            else if (choice == 2)
            {
                var allAlbums = _context.Albums
                    .Include(a => a.Artist)
                    .ToList();

                if (!allAlbums.Any())
                {
                    Console.WriteLine("No albums available.");
                    return;
                }


                // Виводимо список альбомів
                Console.WriteLine("Select an album to view ratings and reviews:");
                int index = 1;
                foreach (var album in allAlbums)
                {
                    Console.WriteLine($"[{index}] {album.AlbumName} by {album.Artist.bandName}");
                    index++;
                }

                // Отримуємо вибір користувача
                if (!int.TryParse(Console.ReadLine(), out int albumIndex) || albumIndex < 1 || albumIndex > allAlbums.Count)
                {
                    Console.WriteLine("Invalid selection.");
                    return;
                }

                var selectedAlbum = allAlbums.ElementAt(albumIndex - 1);

                var albumReviews = _context.RatingsAndReviews
                    .Include(r => r.User)
                    .Where(r => r.AlbumId == selectedAlbum.AlbumId)
                    .ToList();

                if (!albumReviews.Any())
                {
                    Console.WriteLine($"No ratings and reviews found for {selectedAlbum.AlbumName}.");
                    return;
                }


                Console.WriteLine("Sort by:");
                Console.WriteLine("[1] Rating");
                Console.WriteLine("[2] User");
                Console.WriteLine("Enter your choice (1-2, or press Enter for default):");

                string sortChoice = Console.ReadLine();
                IEnumerable<RatingAndReview> sortedReviews = albumReviews;

                switch (sortChoice)
                {
                    case "1":
                        sortedReviews = albumReviews.OrderBy(r => r.Rating);
                        break;
                    case "2":
                        sortedReviews = albumReviews.OrderBy(r => r.User.UserName);
                        break;
                    default:
                        sortedReviews = albumReviews.OrderBy(r => r.Rating);
                        break;
                }

                Console.WriteLine($"Ratings and reviews for {selectedAlbum.AlbumName} by {selectedAlbum.Artist.bandName}:");
                index = 1;
                foreach (var review in albumReviews)
                {
                    Console.WriteLine($"[{index}] User: {review.User.UserName}");
                    Console.WriteLine($"    Rating: {review.Rating}, Comment: {review.Comment ?? "No comment"}");
                    index++;
                }
            }
        }

        public void ArrRatingAndReview(User currentUser) // Method to add a rating and review
        {
            if (currentUser == null)
            {
                Console.WriteLine("User is null.");
                return;
            }
            var allAlbums = _context.Albums
                .Include(a => a.Artist)
                .ToList();
            if (!allAlbums.Any())
            {
                Console.WriteLine("No albums available to rate.");
                return;
            }
            Console.WriteLine("Available Albums:");
            int index = 1;
            foreach (var album in allAlbums)
            {
                Console.WriteLine($"[{index}] {album.AlbumName} - {album.Artist.bandName}");
                index++;
            }
            Console.WriteLine("Enter the album number to rate:");
            if (!int.TryParse(Console.ReadLine(), out int albumIndex) || albumIndex < 1 || albumIndex > allAlbums.Count)
            {
                Console.WriteLine("Invalid album selection.");
                return;
            }
            var selectedAlbum = allAlbums.ElementAt(albumIndex - 1);
            Console.WriteLine("Enter your rating (1-5):");
            if (!int.TryParse(Console.ReadLine(), out int rating) || rating < 1 || rating > 5)
            {
                Console.WriteLine("Invalid rating.");
                return;
            }
            Console.WriteLine("Enter your comment:");
            string comment = Console.ReadLine();
            RatingAndReview newRatingAndReview = new RatingAndReview
            {
                UserId = currentUser.UserId,
                AlbumId = selectedAlbum.AlbumId,
                Rating = rating,
                Comment = comment
            };
            if (currentUser.RatingsAndReviews == null)
            {
                currentUser.RatingsAndReviews = new List<RatingAndReview>();
            }

            currentUser.RatingsAndReviews.Add(newRatingAndReview);
            _context.RatingsAndReviews.Add(newRatingAndReview);
            _context.SaveChanges();
            Console.WriteLine($"Rating and review for '{selectedAlbum.AlbumName}' added successfully.");
        }

        public void AddToUserCollection(User currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("User is null.");
                return;
            }
            var allAlbums = _context.Albums
                .Include(a => a.Artist)
                .ToList();
            if (!allAlbums.Any())
            {
                Console.WriteLine("No albums available to add to your collection.");
                return;
            }
            Console.WriteLine("Available Albums:");
            int index = 1;
            foreach (var album in allAlbums)
            {
                Console.WriteLine($"[{index}] {album.AlbumName} - {album.Artist.bandName}");
                index++;
            }
            Console.WriteLine("Enter the album number to add to your collection:");
            if (!int.TryParse(Console.ReadLine(), out int albumIndex) || albumIndex < 1 || albumIndex > allAlbums.Count)
            {
                Console.WriteLine("Invalid album selection.");
                return;
            }
            var selectedAlbum = allAlbums.ElementAt(albumIndex - 1);

            var existingCollection = _context.UserCollections
            .FirstOrDefault(uc => uc.UserId == currentUser.UserId && uc.AlbumId == selectedAlbum.AlbumId);
            if (existingCollection != null)
            {
                Console.WriteLine($"Album '{selectedAlbum.AlbumName}' is already in your collection as '{existingCollection.Status}'.");
                return;
            }

            Console.WriteLine("Enter status for this album [1]bought, [2]wanted:");

            if (!int.TryParse(Console.ReadLine(), out int choice) || (choice != 1 && choice != 2))
            {
                Console.WriteLine("Invalid selection.");
                return;
            }
            string status = choice == 1 ? "bought" : "wanted";

            // Створюємо новий запис у колекції
            var newCollection = new UserCollection
            {
                AlbumId = selectedAlbum.AlbumId,
                UserId = currentUser.UserId,
                Status = status,
                DateAdded = DateTime.Now,
                Album = selectedAlbum, // Встановлюємо навігаційну властивість
                User = currentUser     // Встановлюємо навігаційну властивість

            };
            if (currentUser.UserCollections == null)
            {
                currentUser.UserCollections = new List<UserCollection>();
            }

            // Додаємо до контексту і до колекції користувача
            currentUser.UserCollections.Add(newCollection);

            // Зберігаємо зміни
            try
            {
                _context.SaveChanges();
                Console.WriteLine($"Album '{selectedAlbum.AlbumName}' added to your collection as '{status}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding album to collection: {ex.Message}");
            }
        } // Method to add an album to the user's collection

        public void ViewUserCollection(User currentUser)  // Method to view the user's collection
        {
            if (currentUser == null)
            {
                Console.WriteLine("User is null.");
                return;
            }
            // Get the user's collection from the database
            var userCollections = _context.UserCollections
            .Include(uc => uc.Album)
            .ThenInclude(a => a.Artist)
            .Where(uc => uc.UserId == currentUser.UserId)
            .ToList();

            if (currentUser.UserCollections == null || !currentUser.UserCollections.Any())
            {
                Console.WriteLine("No albums in your collection.");
                return;
            }

            Console.WriteLine("Sort by:");
            Console.WriteLine("[1] Album name");
            Console.WriteLine("[2] Artist");
            Console.WriteLine("[3] Date added");
            Console.WriteLine("[4] Status");
            Console.WriteLine("Enter your choice (1-4, or press Enter for default):");

            string sortChoice = Console.ReadLine();
            IEnumerable<UserCollection> sortedCollections = userCollections;
            
            switch (sortChoice)
            {
                case "1":
                    sortedCollections = userCollections.OrderBy(uc => uc.Album.AlbumName);
                    break;
                case "2":
                    sortedCollections = userCollections.OrderBy(uc => uc.Album.Artist.bandName);
                    break;
                case "3":
                    sortedCollections = userCollections.OrderBy(uc => uc.DateAdded);
                    break;
                case "4":
                    sortedCollections = userCollections.OrderBy(uc => uc.Status);
                    break;
                default:
                    // Default sorting (no sorting)
                    break;
            }


            Console.WriteLine("Your Collection:");
            int index = 1;
            foreach (var userCollection in sortedCollections)
            {

                Console.WriteLine($"[{index}] Album: {userCollection.Album.AlbumName}, Artist: {userCollection.Album.Artist.bandName}, Status: {userCollection.Status}, Date Added: {userCollection.DateAdded.ToShortDateString()}");
                index++;
            }
        }

        public void AllMusic()
        {
         
            var artists = _context.Artists
                .Include(a => a.Albums)
                .ThenInclude(a => a.Tracks)
                .OrderBy(a => a.bandName)
                .ToList();

            if (!artists.Any())
            {
                Console.WriteLine("No artists found in the database.");
                return;
            }

            Console.WriteLine("=== All Music in Collection ===");
            foreach (var artist in artists)
            {
                Console.WriteLine($"\nArtist: {artist.bandName} ({artist.Country})");
                Console.WriteLine($"Years Active: {artist.yearsOfActivity}");
                Console.WriteLine($"Biography: {artist.Biography}");

                if (!artist.Albums.Any())
                {
                    Console.WriteLine("  No albums found for this artist.");
                    continue;
                }

                foreach (var album in artist.Albums.OrderBy(a => a.ReleaseDate))
                {
                    Console.WriteLine($"\n  Album: {album.AlbumName} (Released: {album.ReleaseDate:yyyy-MM-dd})");
                    Console.WriteLine($"    Label: {album.Label}, Format: {album.Format}");
                    Console.WriteLine($"    Duration: {album.AlbumDuration / 60}m {album.AlbumDuration % 60}s, Tracks: {album.TrackCount}");

                    if (!album.Tracks.Any())
                    {
                        Console.WriteLine("    No tracks found for this album.");
                        continue;
                    }

                    foreach (var track in album.Tracks.OrderBy(t => t.NumberInList))
                    {
                        Console.WriteLine($"    Track {track.NumberInList}: {track.TrackName} ({track.Duration / 60}m {track.Duration % 60}s)");
                        Console.WriteLine($"      Lyrics by: {track.LyricsAuthor}, Music by: {track.MusicAuthor}");
                    }
                }
            }
        }
    }
}
