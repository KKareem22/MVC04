namespace Session04.DAL.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }
        public HealthRecord HealthRecord { get; set; } = default!;
        public ICollection<Booking> MemberSessions { get; set; } = default!;
        public ICollection<MemberShip> MemberPlans { get; set; } = default!;
    }
}
