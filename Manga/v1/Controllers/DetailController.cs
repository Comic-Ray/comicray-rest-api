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
    public class DetailController : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaDetail))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetail(string url)
        {
            try
            {
                return Ok(await MangaDetailParser.Parse(url));
            } catch (Exception ex)
            {
                return BadRequest(new Error { Message = ex.Message, StatusCode = 404 });
            }
        }
    }
}
