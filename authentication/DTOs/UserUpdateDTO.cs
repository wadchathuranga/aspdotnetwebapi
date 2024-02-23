using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace authentication.DTOs
{
    public class UserUpdateDTO
    {
        public string Username { get; set; }
        
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
