using System.ComponentModel.DataAnnotations;

namespace VoteManager.Models.Users
{
    public class AdminUserRegister : UserRegister
    {
        [Required]
        public string Role { get; set; }
    }
}