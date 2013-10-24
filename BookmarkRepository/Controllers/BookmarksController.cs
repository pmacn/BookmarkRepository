using System.Web.Http.Cors;
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
    public class BookmarksController : ApiController
    {
        private readonly IDocumentSession session;

        public BookmarksController(IDocumentSession session)
        {
            this.session = session;
        }

        // GET api/bookmarks
        [Authorize]
        public IHttpActionResult Get()
        {
            var bookmarks = session.Query<Bookmark>()
                                   .Where(b => b.Owner == User.Identity.Name)
                                   .Select(BookmarkDto.Create)
                                   .ToArray();
            return Ok(bookmarks);
        }

        // GET api/bookmarks/id
        [Authorize]
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
        //[EnableCors("*", "Content-Type", "POST")]
        public IHttpActionResult Post(Guid token, [FromBody]BookmarkDto value)
        {
            var user = session.Query<UserProfile>().SingleOrDefault(p => p.BookmarkletToken == token);
            if (user == null)
                return Unauthorized();

            var bookmark = new Bookmark { Name = value.Name, Url = value.Url, Owner = user.UserName };
            session.Store(bookmark);
            session.SaveChanges();
            return Created(Url.Link("DefaultApi", new { Controller = "Bookmarks", Action = "Get", Id = bookmark.Id.ToString() }), Mapper.Map<BookmarkDto>(bookmark));
        }

        [AcceptVerbs("OPTIONS")]
        public HttpResponseMessage Options()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "POST");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            return response;
        }

        // PUT api/bookmarks/5
        [Authorize]
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
        [Authorize]
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if(disposing)
                session.Dispose();
        }
    }
}
