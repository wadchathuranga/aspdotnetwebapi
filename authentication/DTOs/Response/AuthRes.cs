namespace authentication.DTOs.Response
{
    public class AuthRes
    {
        public string Token { get; set; } = string.Empty;

        public bool Result { get; set; }

        public List<string>? Errors { get; set; }
    }
}
