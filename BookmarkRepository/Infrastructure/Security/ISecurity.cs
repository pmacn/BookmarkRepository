using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace BookmarkRepository.Infrastructure.Security
{
    public interface ISecurity
    {
        void CreateAccount(string username, string password);
        bool ChangePassword(string username, string oldPassword, string newPassword);
        bool SignIn(string username, string password, bool persistent);
        void SignOut();
    }

    public class Security : ISecurity
    {
        private readonly IAuthentication authentication;
        private readonly IAccountPersistence persistence;

        public Security(IAuthentication authentication, IAccountPersistence persistence)
        {
            this.authentication = authentication;
            this.persistence = persistence;
        }

        public void CreateAccount(string username, string password)
        {
            var account = new Account {
                Username = username,
                Password = Crypto.HashPassword(password)
            };

            persistence.SaveAccount(account);
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var account = persistence.GetAccount(username);
            if(account == null)
                return false;

            if (!Crypto.VerifyHashedPassword(account.Password, oldPassword))
                return false;
            
            account.Password = Crypto.HashPassword(newPassword);
            persistence.SaveAccount(account);
            return true;
        }

        public bool SignIn(string username, string password, bool persistent)
        {
            if (VerifySignInAttempt(username, password))
            {
                authentication.SignIn(username, persistent);
                return true;
            }

            return false;
        }

        private bool VerifySignInAttempt(string username, string password)
        {
            var account = persistence.GetAccount(username);
            if (account == null)
                return false;

            return Crypto.VerifyHashedPassword(account.Password, password);
        }

        public void SignOut()
        {
            authentication.SignOut();
        }
    }
}