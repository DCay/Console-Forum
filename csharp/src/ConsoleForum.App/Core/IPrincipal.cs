using ConsoleForum.App.Models;

namespace ConsoleForum.App.Core
{
    public interface IPrincipal
    {
        User User { get; }

        void SignIn(User user);

        void SignOut();
    }
}
