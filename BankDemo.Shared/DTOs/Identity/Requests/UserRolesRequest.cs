using BankDemo.Shared.DTOs.Identity;
using System.Collections.Generic;

namespace BankDemo.Shared.DTOs.Identity.Requests;

public class UserRolesRequest
{
    public List<UserRoleDto> UserRoles { get; set; } = new();
}