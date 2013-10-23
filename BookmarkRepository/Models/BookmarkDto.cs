using System;
using AutoMapper;

namespace BookmarkRepository.Models
{
    public class BookmarkDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public static BookmarkDto Create(Bookmark bookmark)
        {
            return Mapper.Map<BookmarkDto>(bookmark);
        }
    }
}