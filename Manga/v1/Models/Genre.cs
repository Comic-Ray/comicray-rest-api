using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ComicAPI.Manga.v1.Models
{
    public class Genre
    {
        [Required]
        public string Name { get; set; }
        [Required] 
        public string Category { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MangaType
    {
        Latest, Newest, TopView
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MangaState
    {
        All, Completed, Ongoing
    }
}
