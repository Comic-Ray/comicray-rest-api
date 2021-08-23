using ComicAPI.comicx.v1.Models;
using RestSharp;
using Supremes.Nodes;
using Supremes.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicAPI.ComicX.v1.Utils
{
    public class HomeParser
    {
        public static async Task<List<ComicDetail.Issue>> ParseIssue(string type)
        {
            var client = new RestClient();
            var response = await client.ExecuteAsync(new RestRequest(BaseUrl));
            if (response.IsSuccessful)
            {
                var document = Parser.HtmlParser.ParseInput(response.Content, BaseUrl);

                var issues = new List<ComicDetail.Issue>();

                var elements = document.GetElementById(type).Children;
                foreach(var e in elements)
                {
                    var title = e.Child(0).Child(0).Child(1).Text;
                    var date = e.Child(0).Child(0).Children.Last().Text;

                    var chapterElement = e.Child(0).Child(1).Child(0).Child(0);

                    var url = chapterElement.Attributes["href"];
                    var name = chapterElement.Text;

                    issues.Add(new ComicDetail.Issue
                    {
                        Title = title, Date = date, RawName = name, Url = url
                    });
                }

                return issues;
            }
            else
            {
                throw response.ErrorException;
            }
        }

        public static async Task<List<ComicSmall>> ParseFeatured()
        {
            var client = new RestClient();
            var response = await client.ExecuteAsync(new RestRequest(BaseUrl));
            if (response.IsSuccessful)
            {
                var comics = new List<ComicSmall>();
                var document = Parser.HtmlParser.ParseInput(response.Content, BaseUrl);
                var elements = document.GetElementById("movie-carousel-top").Children;

                foreach(var e in elements)
                {
                    var url = e.Child(0).Attributes["href"];
                    var title = e.Child(0).Attributes["title"];
                    var image = e.Child(0).Child(0).Child(0).Attr("src");
                    
                    comics.Add(new ComicSmall
                    {
                        Title = title, Url = url, ImageUrl = image
                    });
                }

                return comics;
            }
            else
            {
                throw response.ErrorException;
            }
        }

        private static readonly string BaseUrl = "https://www.comicextra.com";
    }
}
