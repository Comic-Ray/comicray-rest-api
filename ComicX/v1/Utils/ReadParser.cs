using ComicAPI.comicx.v1.Models;
using RestSharp;
using Supremes.Parsers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicAPI.ComicX.v1.Utils
{
    public class ReadParser
    {
        public static async Task<ComicReader> ParseAllPages(string url)
        {
            var client = new RestClient();
            var response = await client.ExecuteAsync(new RestRequest($"{url}/full"));
            if (response.IsSuccessful)
            {
                var document = Parser.HtmlParser.ParseInput(response.Content, url);

                var titleElement = document.GetElementsByClass("chapter-title")[0].Child(0).Child(0);

                var title = titleElement.Child(1).Text;
                var issueTitle = titleElement.Child(0).Text;

                var images = new List<string>();
                var chapters = document.GetElementsByClass("chapter-container")[0].Children.Skip(1);
                foreach(var c in chapters)
                {
                    images.Add(c.Attributes["src"]);
                }

                return new ComicReader
                {
                    Title = title,
                    IssueTitle = issueTitle,
                    Url = url,
                    images = images
                };
            } else
            {
                throw response.ErrorException;
            }
        }
    }
}
