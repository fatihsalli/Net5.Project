using System.ComponentModel.DataAnnotations;

namespace Project.WEB.Models.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Username cannot be blank!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password cannot be blank!")]
        public string Password { get; set; }



    }
}
