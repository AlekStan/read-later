using AutoMapper;
using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReadLater5.Models;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ReadLater5.Controllers
{
    public class BookmarksController : Controller
    {
        private IBookmarkService _bookmarkService;
        private ICategoryService _categoryService;
        private IBookmarkStatisticService _bookmarkStatisticService;
        private readonly IMapper _mapper;
        public BookmarksController(IBookmarkService bookmarkService, ICategoryService categoryService, IBookmarkStatisticService bookmarkStatisticService, IMapper mapper)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
            _bookmarkStatisticService = bookmarkStatisticService;
            _mapper = mapper;
        }

        public async Task<ActionResult<List<BookmarkViewModel>>> Index()
        {
            var bookmarkList = await _bookmarkService.GetBookmarks(User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            var bookmarkViewModelList = _mapper.Map<List<Bookmark>, List<BookmarkViewModel>>(bookmarkList);

            if (bookmarkViewModelList == null) return NotFound();
            return View(bookmarkViewModelList);
        }

        public async Task<ActionResult> Details(int? bookmarkId)
        {
            if (bookmarkId == null)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            Bookmark bookmark = await _bookmarkService.GetBookmark((int)bookmarkId);
            if (bookmark == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            await _bookmarkStatisticService.AddNewBookmarkRecord(bookmark);

            var bookmarkViewModel = _mapper.Map<Bookmark, BookmarkViewModel>(bookmark);
            return View(bookmarkViewModel);
        }

        public IActionResult Create()
        {
            var bookmarkViewModel = new BookmarkViewModel();
            PopulateCreateSelectList(bookmarkViewModel);
            return View(bookmarkViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookmarkViewModel bookmarkViewModel)
        {
            if (bookmarkViewModel == null) return BadRequest();

            if (ModelState.IsValid)
            {
                var bookmark = _mapper.Map<BookmarkViewModel, Bookmark>(bookmarkViewModel);
                bookmark.UserId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                if (_bookmarkService.CreateBookmark(bookmark) != null) return RedirectToAction("Index");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            PopulateCreateSelectList(bookmarkViewModel);
            return View(bookmarkViewModel);
        }

        public async Task<ActionResult> Edit(int? bookmarkId)
        {
            if (bookmarkId == null)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            Bookmark bookmark = await _bookmarkService.GetBookmark((int)bookmarkId);
            if (bookmark == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            var bookmarkViewModel = _mapper.Map<Bookmark, BookmarkViewModel>(bookmark);
            PopulateCreateSelectList(bookmarkViewModel);
            return View(bookmarkViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookmarkViewModel bookmarkViewModel)
        {
            if (bookmarkViewModel == null) return BadRequest();
            if (ModelState.IsValid)
            {
                var bookmark = _mapper.Map<BookmarkViewModel, Bookmark>(bookmarkViewModel);
                if (_bookmarkService.UpdateBookmark(bookmark)) return RedirectToAction("Index");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            PopulateCreateSelectList(bookmarkViewModel);
            return View(bookmarkViewModel);
        }

        public async Task<ActionResult> Delete(int? bookmarkId)
        {
            if (bookmarkId == null)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            Bookmark bookmark = await _bookmarkService.GetBookmark((int)bookmarkId);
            if (bookmark == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            var bookmarkViewModel = _mapper.Map<Bookmark, BookmarkViewModel>(bookmark);
            return View(bookmarkViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int bookmarkId)
        {
            Bookmark bookmark = await _bookmarkService.GetBookmark(bookmarkId);
            if (_bookmarkService.DeleteBookmark(bookmark)) return RedirectToAction("Index");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        [Route("Bookmarks/Share/{bookmarkId}")]
        public async Task<ActionResult> Share(int? bookmarkId)
        {
            if (bookmarkId == null)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            Bookmark bookmark = await _bookmarkService.GetBookmark((int)bookmarkId);
            if (bookmark == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            var bookmarkViewModel = _mapper.Map<Bookmark, BookmarkViewModel>(bookmark);
            return View(bookmarkViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Share(BookmarkViewModel bookmarkViewModel)
        {
            if (bookmarkViewModel == null) return BadRequest();

            if (ModelState.IsValid)
            {
                Bookmark bookmark = await _bookmarkService.GetBookmark((int)bookmarkViewModel.ID);
                bookmark.UserId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                if (await _bookmarkStatisticService.AddNewSharedBookmarkRecord(bookmark))
                {
                    return RedirectToAction("Index");
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return View(bookmarkViewModel);
        }

        private void PopulateCreateSelectList(BookmarkViewModel bookmarkViewModel)
        {
            bookmarkViewModel.Categories = _categoryService.GetCategories(User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value).Select(c =>
                                             new SelectListItem
                                             {
                                                 Text = c.Name,
                                                 Value = c.ID.ToString()
                                             }).ToList();
        }
    }
}
