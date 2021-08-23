using ComicAPI.comicx.v1.Controllers;
using ComicAPI.comicx.v1.Models;
using RestSharp;
using Supremes.Parsers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicAPI.ComicX.v1.Utils
{
    public class DetailParser
    {
        public static async Task<ComicDetail> ParseDetail(string url)
        {
            var client = new RestClient();
            var response = await client.ExecuteAsync(new RestRequest(url));
            if (response.IsSuccessful)
            {
                var document = Parser.HtmlParser.ParseInput(response.Content, url);
                var summary = document.GetElementById("film-content").Text;

                var headerElement = document.GetElementsByClass("movie-l-img")[0];

                var title = headerElement.Child(0).Attributes["alt"];
                var imageUrl = headerElement.Child(0).Attributes["src"];


                var metaInfoElement = document.GetElementsByClass("movie-meta-info")[0].Child(0);

                var status = metaInfoElement.Child(1).Text.Trim();
                var alternateName = metaInfoElement.Child(4).Text.Trim();
                var yr = metaInfoElement.Child(7).Text.Trim().ToInt();

                var author = metaInfoElement.Child(10).Text.Trim();

                var genres = new List<Genre>();
                foreach (var g in metaInfoElement.Child(13).GetAllElements().Skip(1))
                {
                    genres.Add(GenreController.ConvertToGenre(g.Text));
                }

                var issues = new List<ComicDetail.Issue>();
                var episodeElement = document.GetElementsByClass("episode-list")[0];
                foreach (var ep in episodeElement.Child(2).Child(0).Child(0).Children)
                {
                    var a = ep.Child(0).Child(0);
                    var date = ep.Child(1).Text;
                    issues.Add(new ComicDetail.Issue
                    {
                        Title = title,
                        RawName = a.Text,
                        Url = a.Attributes["href"],
                        Date = date
                    });
                }

                var relateds = new List<ComicDetail.Related>();
                var relatedElements = document.GetElementById("list-top-film-week").Children;
                foreach (var r in relatedElements)
                {
                    relateds.Add(new ComicDetail.Related
                    {
                        Title = r.Child(0).Text,
                        Url = r.Child(0).Attributes["href"]
                    });
                }

                return new ComicDetail
                {
                    AlternateName = alternateName, Author = author, Issues = issues, Recommended = relateds, ImageUrl = imageUrl,
                    Genres = genres, Status = status, Summary = summary, Title = title, Url = url, YearOfRelease = yr
                };
            }
            else
            {
                throw response.ErrorException;
            }
        }
    }
}
