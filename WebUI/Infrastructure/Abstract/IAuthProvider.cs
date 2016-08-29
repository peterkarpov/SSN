namespace WebUI.Infrastructure.Abstract
{
    using Domain.Entities;

    public interface IAuthProvider
    {
        bool Authenticate(string username, string password);
        void SignOut();
        bool Registrate(User user);
    }
}