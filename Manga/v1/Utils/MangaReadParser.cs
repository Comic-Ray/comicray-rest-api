using ComicAPI.Manga.v1.Models;
using RestSharp;
using Supremes.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicAPI.Manga.v1.Utils
{
    public class MangaReadParser
    {
        // Pass referer: https://mangakakalot.com as header to read images.
        public static async Task<MangaReader> Parse(string targetUrl)
        {
            var client = new RestClient();
            var response = await client.ExecuteAsync(new RestRequest(targetUrl));
            if (response.IsSuccessful)
            {
                var document = Parser.HtmlParser.ParseInput(response.Content, targetUrl);

                if (new Uri(targetUrl).Host == "readmanganato.com") return ParseMethod2(targetUrl, document);

                var splits = document.GetElementsByClass("current-chapter")[0].Text.Split(":");

                var title = splits[0].Trim();
                var issueName = splits[1].Trim();

                var images = document.GetElementsByClass("container-chapter-reader")[0].Children.Where(c => c.HasAttr("src"))
                    .Select(c => c.Attr("src")).ToList();

                return new MangaReader
                {
                    Title = title, IssueName = issueName, Url = targetUrl, Images = images
                };
            } else
            {
                throw response.ErrorException;
            }
        } 

        private static MangaReader ParseMethod2(string targetUrl, Supremes.Nodes.Document document)
        {
            var infoElement = document.GetElementsByClass("panel-breadcrumb")[0];

            var title = infoElement.Child(2).Text;

            var issueName = infoElement.Child(4).Text;

            var images = document.GetElementsByClass("container-chapter-reader")[0].Children.Where(c => c.HasAttr("src"))
                .Select(c => c.Attr("src")).ToList();

            return new MangaReader
            {
                Images = images, IssueName = issueName, Url = targetUrl, Title = title
            };
        }
    }
}
