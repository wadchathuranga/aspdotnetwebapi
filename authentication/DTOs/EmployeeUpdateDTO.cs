using System.ComponentModel.DataAnnotations;

namespace authentication.DTOs
{
    public class EmployeeUpdateDTO
    {
        public string EmpName { get; set; }
        
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmpEmail { get; set; }

        public string EmpAddess { get; set; }
    }
}
