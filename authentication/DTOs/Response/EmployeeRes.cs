using authentication.Models;

namespace authentication.DTOs.Response
{
    public class EmployeeRes
    {
        public Employee? Data { get; set; }

        public bool isSucceed { get; set; }

        public string? Error { get; set ;}
    }
}
