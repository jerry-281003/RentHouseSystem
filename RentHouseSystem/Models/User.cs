using Microsoft.AspNetCore.Identity;

namespace RentHouseSystem.Models
{
    public class User:IdentityUser
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
