using System.Web.Http.Results;
using AutoMapper;
using BookmarkRepository.Models;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookmarkRepository.Controllers
{
    [Authorize]
    public class BookmarksController : ApiController
    {
        private readonly IDocumentSession session;

        public BookmarksController(IDocumentSession session)
        {
            this.session = session;
        }

        // GET api/bookmarks
        public IHttpActionResult Get()
        {
            var bookmarks = session.Query<Bookmark>()
                                   .Where(b => b.Owner == User.Identity.Name)
                                   .Select(BookmarkDto.Create)
                                   .ToArray();
            return Ok(bookmarks);
        }

        // GET api/bookmarks/id
        public IHttpActionResult Get(int id)
        {
            var bookmark = session.Load<Bookmark>(id);
            if(bookmark == null)
                return NotFound();

            if (bookmark.Owner != User.Identity.Name)
                return Unauthorized();

            return Ok(bookmark);
        }

        // POST api/bookmarks
        public IHttpActionResult Post([FromBody]BookmarkDto value)
        {
            var bookmark = new Bookmark { Name = value.Name, Url = value.Url, Owner = User.Identity.Name };
            session.Store(bookmark);
            session.SaveChanges();
            return Created(Url.Link("DefaultApi", new { Controller = "Bookmarks", Action = "Get", Id = bookmark.Id.ToString() }), Mapper.Map<BookmarkDto>(bookmark));
        }

        // PUT api/bookmarks/5
        public IHttpActionResult Put(int id, [FromBody]BookmarkDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var bookmark = session.Load<Bookmark>(id);
            if(bookmark == null)
                return NotFound();

            if (bookmark.Owner != User.Identity.Name)
                return Unauthorized();

            bookmark.Name = dto.Name;
            bookmark.Url = dto.Url;
            session.SaveChanges();
            return Ok(bookmark);
        }

        // DELETE api/bookmarks/5
        public IHttpActionResult Delete(int id)
        {
            var bookmark = session.Load<Bookmark>(id);
            if(bookmark == null)
                return NotFound();

            if(bookmark.Owner != User.Identity.Name)
                return Unauthorized();

            session.Delete(bookmark);
            session.SaveChanges();
            return Ok(BookmarkDto.Create(bookmark));
        }
    }
}
