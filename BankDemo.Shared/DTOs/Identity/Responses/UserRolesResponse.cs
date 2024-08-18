using System.Collections.Generic;

namespace BankDemo.Shared.DTOs.Identity.Responses;

public class UserRolesResponse
{
    public List<UserRoleDto> UserRoles { get; set; } = new();
}