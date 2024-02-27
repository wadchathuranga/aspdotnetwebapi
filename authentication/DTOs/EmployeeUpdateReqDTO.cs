using System.ComponentModel.DataAnnotations;

namespace authentication.DTOs
{
    public class EmployeeUpdateReqDTO
    {
        public string? Name { get; set; }

        //[EmailAddress]
        //[DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        public string? Address { get; set; }
    }
}
