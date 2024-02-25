namespace authentication.DTOs.Response
{
    public class UserRoleUpdateRes
    {
        public string? Message { get; set; }

        public bool Result { get; set; }

        public List<string>? Errors { get; set; }
    }
}
