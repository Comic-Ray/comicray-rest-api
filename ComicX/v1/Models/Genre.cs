using System.ComponentModel.DataAnnotations;

namespace ComicAPI.comicx.v1.Models
{
    public class Genre
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Tag { get; set; }
    }
}
