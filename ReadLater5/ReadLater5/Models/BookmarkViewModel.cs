using Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReadLater5.Models
{
    public class BookmarkViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Bookmark URL is required")]
        [StringLength(500, ErrorMessage = "URL is too long")]
        [Url]
        public string URL { get; set; }

        [StringLength(250, ErrorMessage = "Description is too long")]
        public string ShortDescription { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public DateTime CreateDate { get; set; }

        public List<SelectListItem> Categories { get; set; }
    }
}
