using System.ComponentModel.DataAnnotations.Schema;

namespace Session04.DAL.Models
{
    public class MemberShip : BaseEntity
    {
        public DateTime EndDate { get; set; }
        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }

        public Plan Plan { get; set; } = default!;
        public int PlanId { get; set; }
        [NotMapped]
        public string Status => EndDate > DateTime.UtcNow ? "Active" : "Expired";
        [NotMapped]
        public bool IsActive => EndDate > DateTime.UtcNow;

    }
}
