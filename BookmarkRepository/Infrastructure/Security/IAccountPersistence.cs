using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookmarkRepository.Infrastructure.Security
{
    public interface IAccountPersistence
    {
        Account GetAccount(string username);
        void SaveAccount(Account account);
    }

    public class RavenAccountPersistence : IAccountPersistence
    {
        private readonly IDocumentSession session;

        public RavenAccountPersistence(IDocumentSession session)
        {
            this.session = session;
        }

        public Account GetAccount(string username)
        {
            return session.Query<Account>().SingleOrDefault(a => a.Username == username);
        }

        public void SaveAccount(Account account)
        {
            session.Store(account);
            session.SaveChanges();
        }
    }
}