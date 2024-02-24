﻿using System.ComponentModel.DataAnnotations;

namespace authentication.DTOs
{
    public class UserRegistrationReqDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
