using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComicAPI.comicx.v1.Models
{
    public class ComicCollection
    {
        [Required]
        public int Page { get; set; }
        [Required] 
        public int TotalPages { get; set; }
        [Required] 
        public List<ComicInfo> Data { get; set; }
    }

    public class ComicInfo
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Url { get; set; }
        [Required] 
        public string ImageUrl { get; set; }
        [Required] 
        public int YearOfRelease { get; set; }
        [Required] 
        public string Status { get; set; }
        public int TotalIssues { get; set; }
        public ComicDetail.Issue LatestIssue { get; set; }
    }

    public class ComicDetail
    {
        [Required]
        public string Title { get; set; }
        [Required] 
        public string ImageUrl { get; set; }
        [Required] 
        public string Url { get; set; }
        [Required] 
        public string Status { get; set; }
        [Required] 
        public string AlternateName { get; set; }
        [Required] 
        public int YearOfRelease { get; set; }
        [Required] 
        public string Author { get; set; }
        [Required] 
        public string Summary { get; set;}
        [Required] 
        public List<Genre> Genres { get; set; }
        [Required] 
        public List<Issue> Issues { get; set; }
        [Required] 
        public List<Related> Recommended { get; set; }

        public class Issue
        {
            [Required]
            public string Title { get; set; }
            [Required]
            public string RawName { get; set; }
            [Required] 
            public string Url { get; set; }
            [Required] 
            public string Date { get; set; }
        }

        public class Related
        {
            [Required]
            public string Title { get; set; }
            [Required]
            public string Url { get; set; }
        }
    }

    public class ComicReader
    {
        [Required] 
        public string Title { get; set; }
        [Required] 
        public string IssueTitle { get; set; }
        [Required] 
        public string Url { get; set; }
        [Required] 
        public List<string> images { get; set; }
    }

    public class ComicSmall
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string ImageUrl { get; set; }

    }
}
