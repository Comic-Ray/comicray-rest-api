using System.Collections.Generic;

namespace ComicAPI.Manga.v1.Models
{
    public class MangaItemCollection
    {
        public MangaType Type { get; set; }
        public MangaState State { get; set; }
        public Genre Category { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public List<MangaItem> Data { get; set; }
    }
    public class MangaItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public long TotalViews { get; set; }
        public Chapter LatestChapter { get; set; }
        public string Summary { get; set; }
        public class Chapter
        {
            public string Title { get; set; }
            public string Url { get; set; }
        }
    }

    public class MangaDetail
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public List<Author> Authors { get; set; }
        public string Status { get; set; }
        public string LastUpdated { get; set; }
        public long TotalViews { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Chapter> Chapters { get; set; }
        public float Rating { get; set; }
        public string Summary { get; set; }

        public class Author
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }

        public class Chapter
        {
            public string Name { get; set; }
            public string Url { get; set; }
            public long TotalViews { get; set; }
            public string UploadDate { get; set; }
        }
    }

    public class MangaReader
    {
        public string Title { get; set; }
        public string Url { get; set; } 
        public string IssueName { get; set; }
        public List<string> Images { get; set; }
    }

    public class MangaPopular
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public Chapter LatestChapter { get; set; }
        public class Chapter
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }

    public class MangaSearchCollection
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public List<MangaSearch> Data { get; set; }
    }

    public class MangaSearch
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public float Rating { get; set; }
        public List<string> Authors { get; set; }
        public string LastUpdated { get; set; }
        public long TotalViews { get; set; }
        public List<Chapter> Chapters { get; set; }

        public class Chapter
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }
}
