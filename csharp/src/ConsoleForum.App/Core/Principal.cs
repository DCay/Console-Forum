using ConsoleForum.App.Models;

namespace ConsoleForum.App.Core
{
    public class Principal : IPrincipal
    {
        public User User { get; private set; }

        public void SignIn(User user)
        {
            this.User = user;
        }

        public void SignOut()
        {
            this.User = null;
        }
    }
}
