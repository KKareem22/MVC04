using System.ComponentModel.DataAnnotations;

namespace Session04.DAL.Models
{
    public class HealthRecord : BaseEntity
    {
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        [Required, MaxLength(5)]
        public string BloodType { get; set; } = default!;
        [MaxLength(500)]
        public string? Note { get; set; }
        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }

    }
}
