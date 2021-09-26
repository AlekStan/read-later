using AutoMapper;
using Entity;
using Microsoft.AspNetCore.Mvc;
using ReadLater5.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReadLater5.Controllers
{
    public class BookmarkStatisticController : Controller
    {
        private IBookmarkStatisticService _bookmarkStatisticService;
        private readonly IMapper _mapper;

        public BookmarkStatisticController(IBookmarkStatisticService bookmarkStatisticService, IMapper mapper)
        {
            _bookmarkStatisticService = bookmarkStatisticService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            var bookmarkStatisticList = await _bookmarkStatisticService.GetBookmarkStatisticList();

            var bookmarkCount = await _bookmarkStatisticService.GetMostPopularLinksForUser(userId);

            var bookmarkStatisticListItemModel = new List<BookmarkStatisticItemViewModel>();

            var resultJoin = bookmarkCount
                .Join(bookmarkStatisticList, arg => arg.bookmarkId, arg => arg.BookmarkId,
                (first, second) =>
                new { BookmarkId = second.BookmarkId, URL = second.Bookmark.URL, Description = second.Bookmark.ShortDescription, ClickCount = first.count, CategoryName = second.Bookmark.Category.Name })
                .Distinct();

            foreach (var item in resultJoin)
            {
                bookmarkStatisticListItemModel.Add(new BookmarkStatisticItemViewModel
                {
                    BookmarkId = item.BookmarkId,
                    ClickCount = item.ClickCount,
                    Bookmark = new Bookmark
                    {
                        ID = item.BookmarkId.Value,
                        URL = item.URL,
                        ShortDescription = item.Description,
                        UserId = userId,
                        Category = new Category
                        {
                            Name = item.CategoryName
                        }
                    }
                });
            }

            var bookmarkStatisticViewModel = new BookmarkStatisticViewModel
            {
                MostPopularLinks = bookmarkStatisticListItemModel,
                SharedLinks = _mapper.Map<List<BookmarkStatistic>, List<BookmarkStatisticItemViewModel>>(await _bookmarkStatisticService.GetSharedLinks()),
                LastSeenLink = _mapper.Map<BookmarkStatistic, BookmarkStatisticItemViewModel>(await _bookmarkStatisticService.GetLastVisitedLink(userId))
            };

            return View(bookmarkStatisticViewModel);
        }
    }
}
