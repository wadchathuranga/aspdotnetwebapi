using System.ComponentModel.DataAnnotations;

namespace authentication.DTOs
{
    public class UserRoleUpdateReqDTO
    {
        [Required(ErrorMessage = "Username is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public required string Email { get; set; }
    }
}
