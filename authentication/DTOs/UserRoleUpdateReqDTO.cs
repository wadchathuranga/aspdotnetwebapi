using System.ComponentModel.DataAnnotations;

namespace authentication.DTOs
{
    public class UserRoleUpdateReqDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
    }
}
