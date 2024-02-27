using System.ComponentModel.DataAnnotations;

namespace authentication.DTOs
{
    public class EmployeeCreateReqDTO
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
