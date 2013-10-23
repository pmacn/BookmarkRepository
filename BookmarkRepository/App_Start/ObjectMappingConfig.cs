using BookmarkRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookmarkRepository
{
    public static class ObjectMappingConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.CreateMap<Bookmark, BookmarkDto>();
        }
    }
}