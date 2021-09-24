using Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IBookmarkService
    {
        Bookmark CreateBookmark(Bookmark bookmark);
        Task<List<Bookmark>> GetBookmarks();
        Task<Bookmark> GetBookmark(int Id);
        bool UpdateBookmark(Bookmark category);
        bool DeleteBookmark(Bookmark category);
    }
}
