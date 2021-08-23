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
    public class HomeController : Controller
    {
        // Does not work

       /* [HttpGet("popular")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MangaPopular>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPopular()
        {
            try
            {
                return Ok(await MangaGenericParser.GetPopular());
            } catch (Exception ex)
            {
                return BadRequest(new Error { StatusCode = 404, Message = ex.Message });
            }
        }*/
    }
}
