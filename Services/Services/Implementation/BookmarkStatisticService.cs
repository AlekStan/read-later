using Data;
using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class BookmarkStatisticService : IBookmarkStatisticService
    {
        private ReadLaterDataContext _readLaterDataContext;

        public BookmarkStatisticService(ReadLaterDataContext readLaterDataContext)
        {
            _readLaterDataContext = readLaterDataContext;
        }

        public async Task<bool> AddNewBookmarkRecord(Bookmark bookmark)
        {
            var bookmarkStatistic = new BookmarkStatistic
            {
                Bookmark = bookmark,
                BookmarkId = bookmark.ID,
                CreationTime = DateTime.Now,
                UserId = bookmark.UserId,
                Seen = true
            };

            _readLaterDataContext.BookmarkStatistic.Add(bookmarkStatistic);

            return await _readLaterDataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddNewSharedBookmarkRecord(Bookmark bookmark)
        {
            var bookmarkStatistic = new BookmarkStatistic
            {
                Bookmark = bookmark,
                BookmarkId = bookmark.ID,
                CreationTime = DateTime.Now,
                Shared = true,
                UserId = bookmark.UserId
            };

            _readLaterDataContext.BookmarkStatistic.Add(bookmarkStatistic);

            return await _readLaterDataContext.SaveChangesAsync() > 0;
        }

        public async Task<List<BookmarkStatistic>> GetBookmarkStatisticList()
        {
            return await _readLaterDataContext.BookmarkStatistic.Include(b => b.Bookmark).Include(b => b.Bookmark.Category).ToListAsync();
        }

        public async Task<BookmarkStatistic> GetLastVisitedLink(string userId)
        {
            return await _readLaterDataContext.BookmarkStatistic.Include(b => b.Bookmark).Include(b => b.Bookmark.Category).Where(x => x.UserId == userId && x.Seen).OrderByDescending(x => x.CreationTime).FirstOrDefaultAsync();
        }

        public async Task<List<BookmarkCount>> GetMostPopularLinksForUser(string userId)
        {
            return await _readLaterDataContext.BookmarkStatistic.Include(b => b.Bookmark).Include(b => b.Bookmark.Category)
                .OrderByDescending(x => x.BookmarkId)
                .Where(x => x.UserId == userId && x.Seen)
                .GroupBy(x => x.BookmarkId,  (key, values) => new { BookmarkId = key, Count = values.Count() })
                .Take(10)
                .Select(x => new BookmarkCount(x.BookmarkId, x.Count))
                .ToListAsync();
        }

        public async Task<List<BookmarkStatistic>> GetSharedLinks()
        {
            return await _readLaterDataContext.BookmarkStatistic.Include(b => b.Bookmark).Include(b => b.Bookmark.Category).Where(x => x.Shared).Distinct().ToListAsync();
        }
    }
}
