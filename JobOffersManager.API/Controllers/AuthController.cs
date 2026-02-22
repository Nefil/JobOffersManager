using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobOffersManager.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto login)
    {
        if (login.Username == "admin" && login.Password == "admin")
            return Ok(GenerateToken(login.Username, "Admin"));

        if (login.Username == "user" && login.Password == "user")
            return Ok(GenerateToken(login.Username, "User"));

        return Unauthorized();
    }

    private object GenerateToken(string username, string role)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("SuperSecretKeyForJwtDemo12345678901!"));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            role
        };
    }
}

public class LoginDto
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}
