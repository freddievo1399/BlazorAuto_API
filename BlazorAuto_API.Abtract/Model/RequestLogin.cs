

using System.ComponentModel.DataAnnotations;

namespace BlazorAuto_API.Abstract
{
    public class RequestLogin
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
