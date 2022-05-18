using System.ComponentModel.DataAnnotations;

namespace IdentityServiceAPI.Dtos
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "You must provide a username.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "You must provide a password.")]
        public string Password { get; set; }
    }
}
