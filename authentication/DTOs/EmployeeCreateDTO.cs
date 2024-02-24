using System.ComponentModel.DataAnnotations;

namespace authentication.DTOs
{
    public class EmployeeCreateDTO
    {
        [Required]
        public string EmpName { get; set; }
        
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmpEmail { get; set; }

        [Required]
        public string EmpAddress { get; set; }
    }
}
