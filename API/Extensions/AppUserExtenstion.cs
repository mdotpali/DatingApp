using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Extensions;

public static class AppUserExtenstion
{
    public static async Task<UserDto> ToDto(this AppUser user, ITokenService tokenService)
    {
        
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            ImageUrl =  user.ImageUrl,
            DisplayName = user.DisplayName,
            Token = await tokenService.CreateToken(user)
        };
    }
}