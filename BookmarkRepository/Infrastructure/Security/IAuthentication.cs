using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace BookmarkRepository.Infrastructure.Security
{
    public interface IAuthentication
    {
        void SignIn(string username, bool persistent);
        void SignOut();
    }

    public class DefaultAuthentication : IAuthentication
    {
        public void SignIn(string username, bool persistent)
        {
            FormsAuthentication.SetAuthCookie(username, persistent);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}