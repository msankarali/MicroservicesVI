using System.ComponentModel.DataAnnotations;

namespace NetCoreWebApp.Dtos
{
    public class SignInDto
    {
        [Required]
        [Display(Name = "E-mail address")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool IsRemember { get; set; }
    }
}