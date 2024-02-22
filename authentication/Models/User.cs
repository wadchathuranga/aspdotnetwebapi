using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace authentication.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; } 
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
