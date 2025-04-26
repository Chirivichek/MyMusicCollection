using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyMusicCollection.Data;
using MyMusicCollection.Entities;
using MyMusicCollection.Interface;

namespace MyMusicCollection.Services
{
    public class AuthService : IAuthService
    {
        // to store database context
        private readonly MusicCollectionBDcontext _context;
        public AuthService(MusicCollectionBDcontext context)
        {
            _context = context;
        }
        public User Login(string email, string password) // Login method
        {
            return _context.Users
                .Include(u => u.UserCollections)
                .ThenInclude(uc => uc.Album)
                .ThenInclude(a => a.Artist)
                .FirstOrDefault(u => u.Email == email && u.Password == password);
        }
        public User Logout() // Logout method
        {
            Console.WriteLine("You have been logged out.");
            // Call authentication again so the user can log in as a different user
            return AuthenticateUser(this);
        }

        public bool Register(string userName, string email, string password, DateTime dateOfBirth) // Register method
        {
            if (_context.Users.Any(u => u.Email == email))
            {
                return false;
            }

            // Create a new user object with the passed data
            var newUser = new User
            {
                UserName = userName,
                Email = email,
                Password = password,
                DateOfBirth = dateOfBirth
            };
            //-----------------------------------------------
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return true;
        }
        public User AuthenticateUser(AuthService authService) // authenticate user method
        {
            while (true)
            {
                Console.WriteLine("Press [1] for Login, or [2] for Register");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter your email:");
                        var email = Console.ReadLine();
                        Console.WriteLine("Enter your password:");
                        var password = Console.ReadLine();
                        var user = authService.Login(email, password);

                        if (user != null)
                        {
                            Console.WriteLine($"Welcome back, {user.UserName}!");
                            return user;
                        }
                        else
                        {
                            Console.WriteLine("Invalid email or password. Please try again.");
                        }
                        break;

                    case "2":
                        Console.WriteLine("Enter your username:");
                        var userName = Console.ReadLine();
                        Console.WriteLine("Enter your email:");
                        email = Console.ReadLine();
                        Console.WriteLine("Enter your password:");
                        password = Console.ReadLine();
                        Console.WriteLine("Enter your date of birth (YYYY-MM-DD):");
                        DateTime dateOfBirth;
                        while (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth))
                        {
                            Console.WriteLine("Invalid date format. Please enter a valid date (YYYY-MM-DD):");
                        }
                        if (authService.Register(userName, email, password, dateOfBirth))
                        {
                            Console.WriteLine("Registration successful! You can now log in.");
                        }
                        else
                        {
                            Console.WriteLine("Email already exists. Please try again.");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please press [1] or [2].");
                        break;
                }
            }
        }
    }
}
