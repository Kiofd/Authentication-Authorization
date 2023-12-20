using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TaskAuthenticationAuthorization.Models.ViewModels
{
    public class RegisterViewModels
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage ="Password don`t match")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
