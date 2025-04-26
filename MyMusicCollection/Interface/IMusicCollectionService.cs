using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMusicCollection_Api.Data;
using MyMusicCollection_Api.Entities;

namespace MyMusicCollection_Api.Interface
{
    internal interface IMusicCollectionService
    {
        void ShowUserInfo(User currentUser);
        void ViewPlaylists(User currentUser);
        void CreatePlaylist(User currentUser);
        void AddTrackToPlaylist(User currentUser);
        void ClearPlaylistTracks(User currentUser);
        void ViewRatingsAndReviews(User currentUser);
        void ArrRatingAndReview(User currentUser);
        void ViewUserCollection(User currentUser);
        void AddToUserCollection(User currentUser);
        void AllMusic();
    }
}
