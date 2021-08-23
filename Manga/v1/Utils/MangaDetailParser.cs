using ComicAPI.Manga.v1.Controllers;
using ComicAPI.Manga.v1.Models;
using RestSharp;
using Supremes.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComicAPI.Manga.v1.Utils
{
    public class MangaDetailParser
    {
        public static async Task<MangaDetail> Parse(string targetUrl)
        {
            var client = new RestClient();
            var response = await client.ExecuteAsync(new RestRequest(targetUrl));
            if (response.IsSuccessful)
            {
                var document = Parser.HtmlParser.ParseInput(response.Content, targetUrl);

                if (new Uri(targetUrl).Host == "readmanganato.com") return ParseMethod2(targetUrl, document);

                var imageUrl = document.GetElementsByClass("manga-info-pic")[0].Child(0).Attr("src");

                var infoElement = document.GetElementsByClass("manga-info-text")[0];

                var title = infoElement.Child(0).Text;
                var authors = infoElement.Child(1).Children.Select(c => new MangaDetail.Author { Name = c.Text, Url = c.Attr("href") }).ToList();

                var status = infoElement.Child(2).Text.Split(":")[1].Trim();
                var lastUpdated = infoElement.Child(3).Text.Split(":")[1].Trim();
                var views = infoElement.Child(5).Text.Split(":")[1].Trim().Replace(",", "").ToLong();

                var genres = infoElement.Child(6).Children.Select(c => GenreController.GetGenreByName(c.Text)).ToList();

                var r = infoElement.Child(8).Child(1).Text;
                var rating = Regex.Match(infoElement.Child(8).Child(1).Text, RegexRating).Groups[1].Value.ToFloat();

                var summaryElement = document.GetElementById("noidungm");
                summaryElement.Child(0).Remove();

                var summary = summaryElement.Text;

                var chapters = new List<MangaDetail.Chapter>();

                var chapterElements = document.GetElementsByClass("chapter-list")[0].Children;
                foreach(var c in chapterElements)
                {
                    var chapterName = c.Child(0).Child(0).Attr("title");
                    var chapterUrl = c.Child(0).Child(0).Attr("href");

                    var chapterViews = c.Child(1).Text.Trim().Replace(",", "").ToLong();
                    var time = c.Child(2).Text.Trim();

                    chapters.Add(new MangaDetail.Chapter
                    {
                        Name = chapterName, Url = chapterUrl, TotalViews = chapterViews, UploadDate = time
                    });
                }

                return new MangaDetail
                {
                    Title = title, Url = targetUrl, Authors = authors, TotalViews = views, Genres = genres, ImageUrl = imageUrl,
                    LastUpdated = lastUpdated, Rating = rating, Status = status, Summary = summary, Chapters = chapters
                };
            } else
            {
                throw response.ErrorException;
            }
        }

        private static MangaDetail ParseMethod2(string targetUrl, Supremes.Nodes.Document document)
        {
            var imageUrl = document.GetElementsByClass("info-image")[0].Child(0).Attr("src");

            var infoElement = document.GetElementsByClass("story-info-right")[0];

            var title = infoElement.Child(0).Text;
            var authors = infoElement.Child(1).Child(0).Child(0).Child(1).Children.Where(c => c.HasAttr("href")).Select(c => new MangaDetail.Author { Name = c.Text, Url = c.Attr("href") }).ToList();

            var status = infoElement.Child(1).Child(0).Child(1).Child(1).Text;

            var genres = infoElement.Child(1).Child(0).Child(2).Child(1).Children.Select(c => GenreController.GetGenreByName(c.Text)).ToList();

            var updated = infoElement.Child(2).Child(0).Text.Split(":")[1].Trim();

            var views = infoElement.Child(2).Child(1).Text.Split(":")[1].Trim().Replace(",", "").ToLong();

            var rating = infoElement.Child(2).Child(3).Child(0).Child(0).Child(1).Child(0).Child(0).Text.ToFloat();

            var summary = document.GetElementsByClass("panel-story-info-description")[0].Text.Replace("Description :", "");

            var chapters = document.GetElementsByClass("row-content-chapter")[0].Children.Select(c =>
            {
                return new MangaDetail.Chapter
                {
                    Name = c.Child(0).Text,
                    Url = c.Child(0).Attr("href"),
                    TotalViews = c.Child(1).Text.Replace(",", "").ToLong(),
                    UploadDate = c.Child(2).Text.Trim()
                };
            }).ToList();


            return new MangaDetail
            {
                Title = title, Authors = authors, Chapters = chapters, Genres = genres, ImageUrl = imageUrl, LastUpdated = updated, Rating = rating, Summary = summary,
                Status = status, TotalViews = views, Url = targetUrl
            };
        }

        private static readonly string RegexRating = @"Mangakakalot\.com\s?rate\s?:\s?([.\d]+)\s?\/\s?5\s?-\s?\d+\s?votes";
    }
}
