using AutoMapper;
using Entity;
using ReadLater5.Models;

namespace ReadLater5.Mapper
{
    public class ControllerMapper : Profile
    {
        public ControllerMapper()
        {
            CreateMap<Bookmark, BookmarkViewModel>().ReverseMap();
        }
    }
}
