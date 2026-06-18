using Session04.DAL.Models.Enums;

namespace Session04.DAL.Models
{
    public class Trainer : GymUser
    {
        public Specialities Specialities { get; set; }
        public ICollection<Session> Sessions { get; set; } = default!;
    }
}
