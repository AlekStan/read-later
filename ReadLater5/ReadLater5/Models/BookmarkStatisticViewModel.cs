using System.Collections.Generic;

namespace ReadLater5.Models
{
    public class BookmarkStatisticViewModel
    {
        public BookmarkStatisticViewModel()
        {
            MostPopularLinks = new List<BookmarkStatisticItemViewModel>();
            SharedLinks = new List<BookmarkStatisticItemViewModel>();
        }
        public List<BookmarkStatisticItemViewModel> MostPopularLinks { get; set; }

        public List<BookmarkStatisticItemViewModel> SharedLinks { get; set; }

        public BookmarkStatisticItemViewModel LastSeenLink { get; set; }
    }
}
