using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class BookmarkStatistic
    {
        [Key]
        public int ID { get; set; }

        public virtual Bookmark Bookmark { get; set; }

        public int? BookmarkId { get; set; }

        public bool Seen { get; set; }

        public bool Shared { get; set; }

        public string UserId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
