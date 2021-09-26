using Entity;
using System;

namespace ReadLater5.Models
{
    public class BookmarkStatisticItemViewModel
    {
        public int ID { get; set; }

        public virtual Bookmark Bookmark { get; set; }

        public int? BookmarkId { get; set; }

        public bool Seen { get; set; }

        public bool Shared { get; set; }

        public string UserId { get; set; }

        public DateTime CreationTime { get; set; }

        public int ClickCount { get; set; }
    }
}
