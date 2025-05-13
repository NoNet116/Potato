using Microsoft.AspNetCore.Identity;

namespace Potato.DbContext.Models.Entity
{
    public class User : IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Image { get; set; }
        public string? Status { get; set; }
        public string? About { get; set; }
        public string GetFullName() => FirstName + " " + LastName;
        public string FullName => $"{FirstName} {LastName}";
    }
}
