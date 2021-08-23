using ComicAPI.Manga.v1.Models;
using RestSharp;
using Supremes.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ComicAPI.Manga.v1.Utils
{
    public class MangaGenericParser
    {
        public static async Task<List<MangaPopular>> GetPopular()
        {
            var client = new RestClient();
            var response = await client.ExecuteAsync(new RestRequest(BaseUrl));
            if (response.IsSuccessful)
            {
                var document = Parser.HtmlParser.ParseInput(response.Content, BaseUrl);

                var populars = new List<MangaPopular>();

                var elements = document.GetElementById("owl-demo").Children;
                foreach(var e in elements)
                {
                    var imageUrl = e.Child(0).Attr("src");
                    var title = e.Child(1).Child(0).Child(0).Text;
                    var url = e.Child(1).Child(0).Child(0).Attr("href");


                    var issueName = e.Child(1).Child(1).Text;
                    var issueUrl = e.Child(1).Child(1).Attr("href");

                    populars.Add(new MangaPopular
                    {
                        Title = title,
                        Url = url,
                        ImageUrl = imageUrl,
                        LatestChapter = new MangaPopular.Chapter
                        {
                            Name = issueName, Url = issueUrl
                        }
                    });
                }

                return populars;
            } else
            {
                throw response.ErrorException;
            }
        }

        public static async Task<MangaSearchCollection> Search(string query, string type, int page)
        {
            var queryEncode = Regex.Replace(Regex.Replace(query, @"[\W+]", "_"), @"[_]+", "_");
            var targetUrl = $"https://manganato.com/{type}/story/{queryEncode}?page={page}";
            var client = new RestClient();
            var response = await client.ExecuteAsync(new RestRequest(targetUrl));
            if (response.IsSuccessful)
            {
                var document = Parser.HtmlParser.ParseInput(response.Content, BaseUrl);

                var mangas = new List<MangaSearch>();

                if (document.GetAllElements().Any(c => c.HasClass("panel-search-story")))
                {
                    var elements = document.GetElementsByClass("panel-search-story")[0].Children;
                    foreach (var e in elements)
                    {
                        try
                        {
                            var imageUrl = e.Child(0).Child(0).Attr("src");
                            var rating = e.Child(0).Child(1).Text.Trim().ToFloat();
                            var title = e.Child(1).Child(0).Child(0).Text;
                            var url = e.Child(1).Child(0).Child(0).Attr("href");

                            var chapters = e.Child(1).Children.Where(c => c.ClassNames.Contains("item-chapter")).Select(c => new MangaSearch.Chapter
                            {
                                Name = c.Text.Trim(),
                                Url = c.Attr("href")
                            }).ToList();

                            var count = e.Child(1).Children.Count();

                            var author = e.Child(1).Child(count - 3).Text.Trim();
                            var updated = e.Child(1).Child(count - 2).Text.Split(":")[1].Trim();
                            var views = e.Child(1).Child(count - 1).Text.Split(":")[1].Trim().Replace(",", "").ToLong();

                            mangas.Add(new MangaSearch
                            {
                                Title = title,
                                Authors = (author.Contains(",") ? author.Split(",").Select(c => c.Trim()).ToList() : new List<string> { author }),
                                Chapters = chapters,
                                Rating = rating,
                                ImageUrl = imageUrl,
                                LastUpdated = updated,
                                TotalViews = views,
                                Url = url
                            });
                        }
                        catch { }
                    }

                    var totalPages = 1;

                    if (mangas.Any() && document.GetAllElements().Any(c => c.HasClass("panel-page-number")))
                    {
                        var panelElement = document.GetElementsByClass("panel-page-number")[0];
                        totalPages = Regex.Match(panelElement.Child(0).Children.Last().Text, @"LAST\((\d+)\)").Groups[1].Value?.ToInt() ?? 1;
                    }

                    return new MangaSearchCollection
                    {
                        Page = page,
                        TotalPages = totalPages,
                        Data = mangas
                    };
                } else
                {
                    return new MangaSearchCollection
                    {
                        Page = page,
                        TotalPages = 1,
                        Data = new List<MangaSearch>()
                    };
                }
            }
            else
            {
                throw response.ErrorException;
            }
        }

        private static readonly string BaseUrl = "https://mangakakalot.com";
    }
}
