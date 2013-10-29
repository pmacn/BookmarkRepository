using BookmarkRepository.Models;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookmarkRepository.Infrastructure
{
    public class ProfileStore
    {
        private readonly IDocumentSession session;

        public ProfileStore(IDocumentSession session)
        {
            this.session = session;
        }

        public UserProfile GetProfile(string username)
        {
            var profile = session.Query<UserProfile>().SingleOrDefault(p => p.Username == username);
            if (profile == null)
            {
                profile = new UserProfile { Username = username };
                session.Store(profile);
                session.SaveChanges();
            }

            return profile;
        }
    }
}