using System.ComponentModel.DataAnnotations;

namespace VoteManager.Models.Tokens
{
    public class TokenRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}