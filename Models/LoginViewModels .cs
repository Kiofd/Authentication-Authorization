using System.ComponentModel.DataAnnotations;

namespace TaskAuthenticationAuthorization.Models.ViewModels
{
    public class LoginViewModels
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}