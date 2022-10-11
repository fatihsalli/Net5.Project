using System.ComponentModel.DataAnnotations;

namespace Project.WEB.Models.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password (Again) is required!")]
        [Compare("Password",ErrorMessage = "Passwords do not match!")]
        public string ConfirmPassword { get; set; }



    }
}
