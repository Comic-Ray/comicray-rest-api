using ComicAPI.comicx.v1.Models;
using ComicAPI.comicx.v1.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace ComicAPI.ComicX.v1.Controllers
{
    [ApiController]
    [Route("v1/comic/[controller]")]
    public class SearchController : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ComicCollection))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSearch([FromQuery(Name = "query")] string query, [FromQuery(Name = "page")] int page = 1)
        {
            try
            {
                return Ok(await PageParser.Parse($"{BaseUrl}/comic-search?key={HttpUtility.UrlEncode(query)}", page));
            }
            catch (PageParserException ex)
            {
                return BadRequest(new Error { StatusCode = ex.Code, Message = ex.Message });
            }
        }

        private static readonly string BaseUrl = "https://www.comicextra.com";
    }
}
