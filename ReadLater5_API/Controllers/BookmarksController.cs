using AutoMapper;
using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ReadLater5_API.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReadLater5_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookmarksController : ControllerBase
    {
        private IBookmarkService _bookmarkService;
        private ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public BookmarksController(IBookmarkService bookmarkService, ICategoryService categoryService, IMapper mapper, LinkGenerator linkGenerator)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [Route("GetBookmarks")]
        public async Task<ActionResult<List<BookmarkModel>>> Index()
        {
            try
            {
                var bookmarkList = await _bookmarkService.GetBookmarks("");

                var bookmarkViewModelList = _mapper.Map<List<Bookmark>, List<BookmarkModel>>(bookmarkList);

                if (bookmarkViewModelList == null) return NotFound();
                return bookmarkViewModelList;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetBookmark/{bookmarkId}")]
        public async Task<ActionResult<BookmarkModel>> Details(int? bookmarkId)
        {
            try
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
                var bookmarkModel = _mapper.Map<Bookmark, BookmarkModel>(bookmark);
                return bookmarkModel;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("CreateBookmark")]
        public ActionResult Create(BookmarkModel bookmarkModel)
        {
            try
            {
                if (bookmarkModel == null) return BadRequest();

                var bookmark = _mapper.Map<BookmarkModel, Bookmark>(bookmarkModel);
                bookmark.UserId = "";
                var createdBookmark = _bookmarkService.CreateBookmark(bookmark);
                if (createdBookmark != null)
                {
                    var location = _linkGenerator.GetPathByAction("Details", "Bookmarks", new { bookmarkId = createdBookmark.ID });
                    return Created(location, bookmarkModel);
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("UpdateBookmark")]
        public ActionResult<BookmarkModel> Edit(BookmarkModel bookmarkModel)
        {
            try
            {
                if (bookmarkModel == null) return BadRequest();

                var bookmark = _mapper.Map<BookmarkModel, Bookmark>(bookmarkModel);
                if (_bookmarkService.UpdateBookmark(bookmark)) return Ok(bookmarkModel);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("DeleteBookmark/{bookmarkId}")]
        public async Task<ActionResult> Delete(int bookmarkId)
        {
            try
            {
                Bookmark bookmark = await _bookmarkService.GetBookmark(bookmarkId);
                if (bookmark == null) return NotFound();
                if (_bookmarkService.DeleteBookmark(bookmark)) return Ok();
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
