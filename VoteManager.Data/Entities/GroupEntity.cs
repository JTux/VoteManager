using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VoteManager.Data.Entities
{
    public class GroupEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string InviteCode { get; set; }

        public List<MembershipEntity> Members { get; set; } = new List<MembershipEntity>();
    }
}