using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IRepository
    {
        IEnumerable<User> Users { get; }
            bool AddUser(User user);
            bool EditUser(User user);

        IEnumerable<Profile> Profiles { get; }
            bool SaveProfile(Profile profile);
            Profile DeleteProfile(Guid? profile);

        IEnumerable<Image> Images { get; }
            bool SaveImage(Image image);
            bool DeleteImage(Guid ImageId);
        IEnumerable<Photobook> Photobooks { get; }
            bool SavePhotobook(Photobook photobook);
        IEnumerable<Friend> Friends { get; }
            bool AddToFriends(Friend Friend);
            bool RemoveFromFriends(Friend Friend);

        IEnumerable<Message> Messages { get; }
            bool SaveMessage(Message message);
        IEnumerable<Talk> Talks { get; }
            bool SaveTalk(Talk Talk);
        IEnumerable<News> News { get; }
            bool SaveNews(News News);
            bool DeleteNews(Guid NewsId);
        IEnumerable<Comment> Comments { get; }
            bool SaveComment(Comment Comment);
        IEnumerable<Like> Likes { get; }
            bool SetLike(Like Like);
        IEnumerable<Dislike> Dislikes { get; }
            bool SetDislike(Dislike Dislike);
    }
}
