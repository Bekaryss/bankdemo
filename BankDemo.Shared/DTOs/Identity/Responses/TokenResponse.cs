using System;

namespace BankDemo.Shared.DTOs.Identity.Responses;

public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);