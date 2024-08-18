using BankDemo.Domain.Entities.Identity;
using BankDemo.Infrastructure.Exceptions;
using BankDemo.Infrastructure.Services.Interfaces;
using BankDemo.Infrastructure.Wrappers;
using BankDemo.Shared.DTOs.Identity;
using BankDemo.Shared.DTOs.Identity.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankDemo.Infrastructure.Services;

public class IdentityService(UserManager<ApplicationUser> userManager) :
    IIdentityService
{
    public async Task<IResult<UserDetailsDto>> GetUserAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        var response = new UserDetailsDto
        {
            Id = Guid.Parse(user.Id),
            Email = user.Email
        };

        return await Result<UserDetailsDto>.SuccessAsync(response);
    }

    public async Task<IResult> RegisterAsync(RegisterRequest request)
    {
        var userWithSameEmail =
            await userManager.Users.FirstOrDefaultAsync(x => x.Email.Equals(request.Email));

        if (userWithSameEmail != null)
        {
            throw new IdentityException("The user is already registered!");
        }

        var userToCreate = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email,
            RefreshToken = string.Empty
        };

        var userToCreateResult = await userManager.CreateAsync(userToCreate, request.Password);
        if (!userToCreateResult.Succeeded)
        {
            throw new CustomException("Failed to register, try again later!");
        }

        return await Result<Guid>.SuccessAsync(Guid.Parse(userToCreate.Id), "Successful registration");
    }
}