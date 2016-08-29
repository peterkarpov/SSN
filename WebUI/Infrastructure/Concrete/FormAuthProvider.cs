namespace WebUI.Infrastructure.Concrete
{
    using Domain;
    using Domain.Entities;
    using System.Linq;
    using System.Web.Security;
    using WebUI.Infrastructure.Abstract;

    public class FormAuthProvider : IAuthProvider
    {
        private IRepository repository;

        public FormAuthProvider(IRepository repo)
        {
            repository = repo;
        }

        public bool Authenticate(string username, string password)
        {
            var user = repository.Users.FirstOrDefault(x => x.login == username);

            if (user == null) return false;

            bool result = user.password == password;
            
            //rewrite as use cookie context???

            username += "|";
            username += user.UserId.ToString();

            if (result)
                FormsAuthentication.SetAuthCookie(username, false);
            
            return result;
        }

        public bool Registrate(User user)
        {
            if (repository.Users.FirstOrDefault(u => u.login == user.login) != null) return false;

            repository.AddUser(user);
            return Authenticate(user.login, user.password);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}