using System.ComponentModel.DataAnnotations;

namespace authentication.DTOs
{
    public class LoginInputDTO
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
