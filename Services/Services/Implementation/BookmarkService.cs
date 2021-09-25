using Data;
using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Services.Implementation
{
    public class BookmarkService : IBookmarkService
    {
        private ReadLaterDataContext _readLaterDataContext;

        public BookmarkService(ReadLaterDataContext readLaterDataContext)
        {
            _readLaterDataContext = readLaterDataContext;
        }

        public Bookmark CreateBookmark(Bookmark bookmark)
        {
            bookmark.CreateDate = DateTime.Now;
            var category = _readLaterDataContext.Categories.FirstOrDefault(c => c.ID == bookmark.CategoryId);
            bookmark.Category = category;

            _readLaterDataContext.Add(bookmark);
            if (_readLaterDataContext.SaveChanges() > 0) return bookmark;
            return null;
        }

        public bool DeleteBookmark(Bookmark bookmark)
        {
            _readLaterDataContext.Bookmark.Remove(bookmark);
            return _readLaterDataContext.SaveChanges() > 0;
        }

        public async Task<Bookmark> GetBookmark(int Id)
        {
            return await _readLaterDataContext.Bookmark.Include(b => b.Category).Where(b => b.ID == Id).FirstOrDefaultAsync();
        }

        public async Task<List<Bookmark>> GetBookmarks(string userId)
        {
            return await _readLaterDataContext.Bookmark.Where(b => b.UserId == userId).Include(b => b.Category).ToListAsync();
        }

        public bool UpdateBookmark(Bookmark bookmark)
        {
            var category = _readLaterDataContext.Categories.FirstOrDefault(c => c.ID == bookmark.CategoryId);
            bookmark.Category = category;

            _readLaterDataContext.Update(bookmark);
            return _readLaterDataContext.SaveChanges() > 0;
        }
    }
}
