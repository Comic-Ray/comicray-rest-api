using ComicAPI.comicx.v1.Controllers;
using ComicAPI.comicx.v1.Models;
using ComicAPI.ComicX.v1.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Supremes.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ComicAPI.ComicX.v1.Controllers
{
    [ApiController]
    [Route("v1/comic/[controller]")]
    public class DetailController : Controller
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ComicDetail))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUrl([FromQuery(Name = "url")] string url)
        {
            try
            {
                return Ok(await DetailParser.ParseDetail(url));
            } catch(Exception ex)
            {
                return BadRequest(new Error { StatusCode = 404, Message = ex.Message } );
            }
        }
    }
}
