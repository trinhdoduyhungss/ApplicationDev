using ApplicationDev.Models;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser: IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public virtual Store? Store { get; set; }
    public IEnumerable<OrderItem>? OderItems { get; set; }
}