using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using BookmarkRepository.Models;
using Raven.Client;
using System.Web.Helpers;

namespace BookmarkRepository.Infrastructure
{
    public class WebSecurity
    {
        public static IDocumentStore Store { get { return DependencyConfig.Get<IDocumentStore>(); } }

        public static bool Login(string userName, string password, bool persistCookie = false)
        {
            if (String.IsNullOrWhiteSpace(userName) || String.IsNullOrWhiteSpace(password))
                return false;

            using (var session = Store.OpenSession())
            {
                var user = session.Query<UserProfile>().SingleOrDefault(p => p.UserName == userName);
                if (user == null)
                    return false;

                if (!Crypto.VerifyHashedPassword(user.Password, password))
                    return false;

                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(userName), user.Roles.ToArray());
                return true;
            }
        }

        public static void Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.User = null;
        }

        public static string CreateUserAndAccount(string userName, string password)
        {
            if (String.IsNullOrWhiteSpace(userName))
                throw new MembershipCreateUserException(MembershipCreateStatus.InvalidUserName);
            if (String.IsNullOrWhiteSpace(password))
                throw new MembershipCreateUserException(MembershipCreateStatus.InvalidPassword);

            using (var session = Store.OpenSession())
            {
                if (session.Query<UserProfile>().Any(p => p.UserName == userName))
                    throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateUserName);

                session.Store(
                    new UserProfile {
                        UserName = userName,
                        Password = Crypto.HashPassword(password)
                });
                session.SaveChanges();
                return String.Empty; // todo: not dealing with confirmation tokens yet
            }
        }

        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            using (var session = Store.OpenSession())
            {
                var user = session.Query<UserProfile>().SingleOrDefault(p => p.UserName == userName);
                if (user == null)
                    return false;
                if (!Crypto.VerifyHashedPassword(user.Password, oldPassword))
                    return false;

                user.Password = Crypto.HashPassword(newPassword);
                session.SaveChanges();
                return true;
            }
        }
    }
}