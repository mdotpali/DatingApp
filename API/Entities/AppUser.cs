using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppUser: IdentityUser
{

    [MaxLength(200)]
    public required string DisplayName { get; set; }
    
    [MaxLength(200)]
    public string? ImageUrl { get; set; }
    
    [MaxLength(200)]
    public string? RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpiry { get; set; }
    public Member Member { get; set; } = null!;
}