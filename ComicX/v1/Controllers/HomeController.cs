using ComicAPI.comicx.v1.Models;
using ComicAPI.ComicX.v1.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicAPI.ComicX.v1.Controllers
{
    [ApiController]
    [Route("v1/comic/[controller]")]
    public class HomeController : Controller
    {
        [HttpGet("featured")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ComicSmall))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFeatured()
        {
            try
            {
                return Ok(await HomeParser.ParseFeatured());
            } catch(Exception ex)
            {
                return BadRequest(new Error { StatusCode = 404, Message = ex.Message });
            }
        }

        [HttpGet("ongoing")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ComicDetail.Issue>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNewReleases()
        {
            try
            {
                return Ok(await HomeParser.ParseIssue("new-release"));
            }
            catch (Exception ex)
            {
                return BadRequest(new Error { StatusCode = 404, Message = ex.Message });
            }
        }

        [HttpGet("completed")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ComicDetail.Issue>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCompleted()
        {
            try
            {
                return Ok(await HomeParser.ParseIssue("top-week"));
            }
            catch (Exception ex)
            {
                return BadRequest(new Error { StatusCode = 404, Message = ex.Message });
            }
        }
    }
}
