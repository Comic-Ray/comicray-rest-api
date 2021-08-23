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
    public class GenreController : Controller
    {
        [HttpGet("list")]
        public IEnumerable<Genre> GetGenreList()
        {
            return GetAllGenre();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaItemCollection))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByGenre([FromQuery(Name = "type")] MangaType type, [FromQuery(Name = "state")] MangaState state, [FromQuery(Name = "category")] string category, [FromQuery(Name = "Page")] int page = 1)
        {
            try
            {
                var genres = GetGenreList();
                var genre = genres.FirstOrDefault(c => c.Category == category) ?? genres.First();
                return Ok(await MangaListParser.Parse(type, state, genre, page));
            } catch(Exception ex)
            {
                return BadRequest(new Error { StatusCode = 404, Message = ex.Message });
            }
        }

        public static IEnumerable<Genre> GetAllGenre() => GenreList.Select(c =>
        {
            var splits = c.Split("=");
            return new Genre { Name = splits[0], Category = splits[1] };
        });
        public static Genre GetGenreByName(string name) => GetAllGenre().First(c => c.Name.ToLower() == name.ToLower());

        private static readonly string[] GenreList = new[]
        {
            "All=all", "Action=2", "Adult=3", "Adventure=4", "Comedy=6", "Cooking=7", "Doujinshi=9", "Drama=10", "Ecchi=11", "Fantasy=12", "Gender bender=13", "Harem=14",
            "Historical=15", "Horror=16", "Isekai=45", "Josei=17", "Manhua=44", "Manhwa=43", "Martial Arts=19", "Mature=20", "Mecha=21", "Medical=22", "Mystery=24", "One shot=25",
            "Psychological=26", "Romance=27", "School life=28", "Sci-Fi=29", "Sci fi=29", "Seinen=30", "Shoujo=31", "Shoujo ai=32", "Shounen=33", "Shounen ai=34", "Slice of life=35", "Smut=36",
            "Sports=37", "Supernatural=38", "Tragedy=39", "Webtoons=40", "Yaoi=41", "Yuri=42"
        };
    }
}
