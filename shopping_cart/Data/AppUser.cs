using Microsoft.AspNetCore.Identity;

namespace shopping_cart.Data
{
    public class AppUser: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set;}
    }
}
