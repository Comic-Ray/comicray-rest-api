using ComicAPI.comicx.v1.Models;
using ComicAPI.ComicX.v1.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ComicAPI.ComicX.v1.Controllers
{
    [ApiController]
    [Route("v1/comic/[controller]")]
    public class ReadController : Controller
    {
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ComicReader))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll([FromQuery(Name = "url")] string url)
        {
            try
            {
                return Ok(await ReadParser.ParseAllPages(url));
            } catch(Exception ex)
            {
                return BadRequest(new Error { StatusCode = 404, Message = ex.Message });
            }
        }
    }
}
