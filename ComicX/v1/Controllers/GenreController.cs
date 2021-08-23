using ComicAPI.comicx.v1.Models;
using ComicAPI.comicx.v1.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComicAPI.comicx.v1.Controllers
{
    [ApiController]
    [Route("v1/comic/[controller]")]
    public class GenreController : Controller
    {

        [HttpGet("list")]
        public IEnumerable<Genre> GetGenreList()
        {
            return GenreList.OrderBy(c => c).Select(c => ConvertToGenre(c)); 
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ComicCollection))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByTag([FromQuery(Name = "tag")] string tag, [FromQuery(Name = "page")] int page = 1)
        {
            try
            {
                var data = await PageParser.Parse($"{BaseUrl}/{tag}");
                return Ok(data);
            }
            catch (PageParserException ex)
            {
                return BadRequest(new Error { StatusCode = ex.Code, Message = ex.Message });
            }
        }

        public static Genre ConvertToGenre(string genre) => new Genre { Name = genre, Tag = Regex.Replace(genre.ToLower(), @"\s+", "-") + "-comic" };

        private static readonly string[] GenreList = new[]
        {
            "Adventure", "Superhero", "Action", "Marvel", "DC Comics", "Sci-Fi", "Fantasy", "Horror", "Movies & TV", "Comedy",
            "Graphic Novels", "Crime", "Supernatural", "Mature", "Leading Ladies", "Drama", "Suspense", "Mystery", "Children",
            "Military", "Video Games", "Slice of Life", "Romance", "Anthropomorphic", "Literature", "Historical", "Martial Arts",
            "Pulp", "Western", "Robots", "Mythology", "Zombies", "Vampires", "Anthology", "Spy", "Post-Apocalyptic", "War",
            "Political", "Thriller", "Manga", "Biography", "LGBTQ", "Family", "School Life", "Music", "Religious", "Sport", "Popular"
        };

        private static readonly string BaseUrl = "https://www.comicextra.com";
    }
}
