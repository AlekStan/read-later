using AutoMapper;
using Entity;
using ReadLater5_API.Models;

namespace ReadLater5_API.Mapper
{
    public class ControllerMapper : Profile
    {
        public ControllerMapper()
        {
            CreateMap<Bookmark, BookmarkModel>().ReverseMap();
        }
    }
}
