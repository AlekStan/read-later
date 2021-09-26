using Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IBookmarkStatisticService
    {
        Task<List<BookmarkStatistic>> GetBookmarkStatisticList();

        Task<List<BookmarkCount>> GetMostPopularLinksForUser(string userId);

        Task<List<BookmarkStatistic>> GetSharedLinks();

        Task<BookmarkStatistic> GetLastVisitedLink(string userId);

        Task<bool> AddNewBookmarkRecord(Bookmark bookmark);

        Task<bool> AddNewSharedBookmarkRecord(Bookmark bookmark);
    }

    public record BookmarkCount(int? bookmarkId, int count);
}
