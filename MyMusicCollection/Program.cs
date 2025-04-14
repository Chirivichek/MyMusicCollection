using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyMusicCollection.Data;
using MyMusicCollection.Entitys;
using MyMusicCollection.Interface;
using MyMusicCollection.Services;
using MyMusicCollection.Entitys;
using System.Configuration;

namespace MyMusicCollection
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //if (args.Length > 0 && args[0].ToLower().Contains("ef"))
            //{

            //    return;
            //}

            //IConfiguration configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .Build();


            //string connectionString = configuration.GetConnectionString("MusicCollectionDb");
            //var optionsBuilder = new DbContextOptionsBuilder<MusicCollectionBDcontext>();
            //optionsBuilder.UseSqlServer(connectionString);

            var optionsBuilder = new DbContextOptionsBuilder<MusicCollectionBDcontext>();
            string connectionString = ConfigurationManager.ConnectionStrings["MusicCollectionDb"].ConnectionString;
            optionsBuilder.UseSqlServer(connectionString);

            using (var context = new MusicCollectionBDcontext(optionsBuilder.Options))
            {

                var authService = new AuthService(context);
                IAuthService authService1 = new AuthService(context);

                Console.WriteLine("--------------- Welcome to the Music Collection!----------------");
                Console.WriteLine();

                User currentUser = authService1.AuthenticateUser(authService);

                if (currentUser != null)
                {

                    currentUser = context.Users
                    .Include(u => u.PlayLists)
                    .ThenInclude(p => p.Tracks)
                    .Include(u => u.UserCollections)
                    .ThenInclude(uc => uc.Album)
                    .ThenInclude(a => a.Artist)
                    .FirstOrDefault(u => u.UserId == currentUser.UserId);

                    if (currentUser == null)
                    {
                        Console.WriteLine("User not found after loading playlists.");
                        return;
                    }

                    ShowMainMenu(context, currentUser);
                }
            }
        }


        static void ShowMainMenu(MusicCollectionBDcontext _context, User currentUser)
        {
            IMusicCollectionService musicCollectionService = new MusicCollectionService(_context);
            var authService = new AuthService(_context);
            while (true)
            {
                Console.WriteLine("\n--------------- Main Menu ----------------");
                Console.WriteLine($"[s] Search");
                Console.WriteLine("[1] View User info");
                Console.WriteLine("[2] View playlists");
                Console.WriteLine("[3] Create playlist");
                Console.WriteLine("[4] Add track to playlist");
                Console.WriteLine("[5] Delete playlist track");
                Console.WriteLine("[6] View ratings and reviews");
                Console.WriteLine("[7] Add rating and review");
                Console.WriteLine("[8] View user collection");
                Console.WriteLine("[9] Add to collection");
                Console.WriteLine("[0] Exit");
                Console.WriteLine("[*] Logout");
                var choice = Console.ReadLine();


                switch (choice)
                {
                    case "s":      // Call method to search track or album
                        musicCollectionService.ShowUserInfo(currentUser);
                        break;
                    case "1":       // Call method to view user info
                        musicCollectionService.ShowUserInfo(currentUser);
                        break;
                    case "2":      // Call method to view playlists
                        musicCollectionService.ViewPlaylists(currentUser);
                        break;
                    case "3":       // Call method to create playlist
                        musicCollectionService.CreatePlaylist(currentUser);
                        break;
                    case "4":       // Call method to add track to playlist
                        musicCollectionService.AddTrackToPlaylist(currentUser);
                        break;
                    case "5":    // Call method to delete playlist track
                        musicCollectionService.ClearPlaylistTracks(currentUser);
                        break;
                    case "6": // Call method to view ratings and reviews
                        musicCollectionService.ViewRatingsAndReviews(currentUser);
                        break;
                    case "7": // Call method to add rating and review
                        musicCollectionService.ArrRatingAndReview(currentUser);
                        break;
                    case "8": // Call method to view user collection
                        musicCollectionService.ViewUserCollection(currentUser);
                        break;
                    case "9":// Call method to add to user collection
                        musicCollectionService.AddToUserCollection(currentUser);
                        break;
                    case "*": // Logout
                              // Викликаємо логаут і оновлюємо currentUser
                        currentUser = authService.Logout();
                        if (currentUser == null)
                        {
                            Console.WriteLine("Authentication failed. Exiting...");
                            return;
                        }
                        // Перезавантажуємо пов’язані дані для нового користувача
                        currentUser = _context.Users
                            .Include(u => u.PlayLists)
                            .ThenInclude(p => p.Tracks)
                            .Include(u => u.UserCollections)
                            .ThenInclude(uc => uc.Album)
                            .ThenInclude(a => a.Artist)
                            .FirstOrDefault(u => u.UserId == currentUser.UserId);
                        if (currentUser == null)
                        {
                            Console.WriteLine("User not found after logout.");
                            return;
                        }
                        break;
                    case "0":// Exit the application
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}