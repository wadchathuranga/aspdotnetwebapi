using System.ComponentModel.DataAnnotations;

namespace authentication.DTOs
{
    public class UserInputDTO
    {
        public string? Id { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
