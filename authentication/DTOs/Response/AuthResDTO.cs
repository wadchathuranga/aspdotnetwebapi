namespace authentication.DTOs.Response
{
    public class AuthResDTO
    {
        public string? Token { get; set; }

        public bool isSucceed { get; set; }

        public string? Error { get; set; }
    }
}
