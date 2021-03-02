using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VoteManager.Data.Resources;

namespace VoteManager.Data.Entities
{
    public class MembershipEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public UserEntity User { get; set; }

        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }
        public GroupEntity Group { get; set; }

        [Required]
        public MembershipStatus Status { get; set; }
    }
}