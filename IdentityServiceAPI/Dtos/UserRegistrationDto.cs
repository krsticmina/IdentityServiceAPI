using System.ComponentModel.DataAnnotations;

namespace IdentityServiceAPI.Dtos
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "You must provide a username.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "You must provide an email.")]
        [MaxLength(100)]
        [RegularExpression(@"(.*)@(.*)\.(.*)", ErrorMessage = "Input email not valid.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "You must provide a name.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "You must provide a role.")]
        [EnumDataType(typeof(Position), ErrorMessage = "Must be Admin, Manager or Employee")]
        public string Role { get; set; }
        [Required(ErrorMessage = "You must provide a password.")]
        [MinLength(8, ErrorMessage = "A password must be at least 8 characters long.")]
        public string Password { get; set; }
        public string? Salt { get; set; }
    }
}
