using System.ComponentModel.DataAnnotations;

namespace ComicAPI.comicx.v1.Models
{
    public class Error
    {
        [Required]
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
