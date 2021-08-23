using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ComicAPI.comicx.v1.Models;
using RestSharp;
using Supremes.Parsers;

namespace ComicAPI.comicx.v1.Utils
{
    public class PageParser
    {
        /// <summary>
        /// 
        /// Throws <see cref="PageParserException"/> on failure resonse.
        /// </summary>
        /// <param name="url">Url to fetch from</param>
        /// <returns></returns>
        public static async Task<ComicCollection> Parse(string url, int page = 1)
        {
            var comics = new List<ComicInfo>();
            var client = new RestClient($"{url}" + (page > 1 ? $"/{page}" : ""));
            client.FollowRedirects = false;
            var response = await client.ExecuteAsync(new RestRequest(Method.GET));
            if (response.IsSuccessful)
            {
                var document = Parser.HtmlParser.ParseInput(response.Content, url);
                var elements = document.GetElementsByClass("cartoon-box");
                Debug.WriteLine(elements.First().Text.Trim());
                if (elements.First().Text.Trim() == "Not found")
                {
                    return new ComicCollection
                    {
                        Page = page,
                        TotalPages = 0,
                        Data = new List<ComicInfo>()
                    };
                }

                foreach (var element in elements)
                {
                    var title = element.Child(1).Child(0).Text;
                    var targetUrl = element.Child(0).Attributes["href"];
                    var imageUrl = element.Child(0).Child(0).Attributes["src"];
                    var status = element.Child(1).Child(2).Text.Replace("Stasus:", "").Trim();
                    var yr = Regex.Match(element.Child(1).Child(3).Text, @"\d+").Value.ToInt();

                    var comic = new ComicInfo
                    {
                        Title = title,
                        ImageUrl = imageUrl,
                        Url = targetUrl,
                        Status = status,
                        YearOfRelease = yr
                    };

                    if (element.Child(1).Child(1).Text.StartsWith("Latest:"))
                    {
                        var inside = element.Child(1).Child(1).Child(0);
                        comic.LatestIssue = new ComicDetail.Issue
                        {
                            Title = title,
                            RawName = inside.Text,
                            Url = inside.Attributes["href"],
                            Date = "Latest"
                        };
                    } else
                    {
                        comic.TotalIssues = Regex.Match(element.Child(1).Child(1).Text, @"\d+").Value.ToInt();
                    }

                    comics.Add(comic);
                }

                var totalPages = 7; // let's just return a constant //Regex.Matches(response.Content, $"{url}/\\d+").Count;

                if (url.Contains("comic-search?")) // this is a search query
                {
                    totalPages = 1 + document.GetElementsByClass("general-nav")[1].Children.Skip(1).Select(c => c.Attributes["href"]).Distinct().Count();
                }

                return new ComicCollection
                {
                    Page = page,
                    TotalPages = totalPages,
                    Data = comics
                };
            } else
            {
                throw new PageParserException((int)response.StatusCode, response.ErrorMessage);
            }
        }
    }

    public class PageParserException : Exception
    {
        public int Code { get; private set; }
        public override string Message { get; }
        public PageParserException(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}
