using ComicAPI.Manga.v1.Models;
using ComicAPI.Manga.v1.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicAPI.Manga.v1.Controllers
{
    [ApiController]
    [Route("v1/manga/[controller]")]
    public class SearchController : Controller
    {
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaSearchCollection))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSearch([FromQuery(Name = "query")] string query, [FromQuery(Name = "page")] int page = 1)
        {
            try
            {
                return Ok(await MangaGenericParser.Search(query, "search", page));
            }
            catch (Exception ex)
            {
                return BadRequest(new Error { StatusCode = 404, Message = ex.Message });
            }
        }

        [HttpGet("author")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaSearchCollection))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSearchByAuthor([FromQuery(Name = "authorUrl")] string url, [FromQuery(Name = "page")] int page = 1)
        {
            try
            {
                return Ok(await MangaGenericParser.Search(new Uri(url).Segments.Last(), "author", page));
            }
            catch (Exception ex)
            {
                return BadRequest(new Error { StatusCode = 404, Message = ex.Message });
            }
        }
    }
}
