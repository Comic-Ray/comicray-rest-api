using ComicAPI.Manga.v1.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using Supremes.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComicAPI.Manga.v1.Utils
{
    public class MangaListParser
    {
        public static async Task<MangaItemCollection> Parse(MangaType type, MangaState state, Genre category, int page = 1) 
        {
            var targetUrl = $"https://mangakakalot.com/manga_list?type={type}&category={category.Category}&state={state}&page={page}";
            var client = new RestClient();
            var response = await client.ExecuteAsync(new RestRequest(targetUrl));
            if (response.IsSuccessful)
            {
                var document = Parser.HtmlParser.ParseInput(response.Content, targetUrl);
                var elements = document.GetElementsByClass("truyen-list")[0].Children.Skip(1);

                var mangas = new List<MangaItem>();

                foreach(var e in elements)
                {
                    try
                    {
                        var url = e.Child(0).Attributes["href"];
                        var title = e.Child(1).Text;
                        var imageUrl = e.Child(0).Child(0).Attr("src");
                        var chapterName = e.Child(2).Text;
                        var chapterUrl = e.Child(2).Attr("href");
                        var viewCount = e.Child(3).Text.Replace(",", "").ToLong();
                        var summary = e.Children.Last().Text;

                        mangas.Add(new MangaItem
                        {
                            Title = title,
                            ImageUrl = imageUrl,
                            Summary = summary,
                            TotalViews = viewCount,
                            Url = url,
                            LatestChapter = new MangaItem.Chapter
                            {
                                Title = chapterName,
                                Url = chapterUrl
                            }
                        });
                    } catch { /*ignore*/ }
                }

                var panelElement = document.GetElementsByClass("panel_page_number")[0];
                var totalPages = Regex.Match(panelElement.Child(0).Children.Last().Text, @"Last\((\d+)\)").Groups[1].Value?.ToInt() ?? 0;

                return new MangaItemCollection
                {
                    Page = page,
                    TotalPages = totalPages,
                    Category = category,
                    State = state,
                    Type = type,
                    Data = mangas
                };
            } else
            {
                throw response.ErrorException;
            }
        }
    }
}
