using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMusicCollection_Api.Entities;
using MyMusicCollection_Api.Services;

namespace MyMusicCollection_Api.Interface
{
    internal interface IAuthService
    {
        User Login(string email, string password);
        User Logout();
        bool Register(string userName, string email, string password, DateTime dateOfBirth);
        User AuthenticateUser(AuthService authService);
    }
}
