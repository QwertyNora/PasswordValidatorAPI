using Microsoft.AspNetCore.Mvc;
using PasswordValidatorApi.Data;
using PasswordValidatorApi.Dtos;
using PasswordValidatorApi.Models;
using PasswordValidatorApi.Services;
using System.Security.Cryptography;
using System.Text;

namespace PasswordValidatorApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PasswordController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PasswordService _passwordService;

    public PasswordController(AppDbContext context, PasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidatePassword([FromBody] PasswordRequestDto request)
    {
        var errors = _passwordService.ValidatePassword(request.Password);
        bool isValid = errors.Count == 0;

        string passwordHash = _passwordService.HashPassword(request.Password);

        var attempt = new ValidationAttempt
        {
            PasswordHash = passwordHash,
            IsValid = isValid,
            ValidationErrors = string.Join(", ", errors),
            Timestamp = DateTime.UtcNow
        };

        _context.ValidationAttempts.Add(attempt);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            isValid,
            errors
        });
    }


}
