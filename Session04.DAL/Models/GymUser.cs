using Session04.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Session04.DAL.Models
{
    public abstract class GymUser : BaseEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = default!;
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = default!;
        [Required, MaxLength(11)]
        [RegularExpression(@"^(010|011}012|015)\d{8}$", ErrorMessage = "Egyptian Phone Format Only")]
        public string Phone { get; set; } = default!;
        public DateOnly DateOFBirth { get; set; }
        public Gender Gender { get; set; }

        public Address Address { get; set; } = default!;


    }

    [Owned]
    public class Address
    {
        public int BuildingNumber { get; set; }
        [Required, MaxLength(30)]
        public string City { get; set; } =default!;
        [Required, MaxLength(30)]
        public string Street { get; set; } = default!;

    }
}
