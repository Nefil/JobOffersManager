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
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto login)
    {
        var adminUsername = _configuration["Users:Admin:Username"];
        var adminPassword = _configuration["Users:Admin:Password"];
        var userUsername = _configuration["Users:User:Username"];
        var userPassword = _configuration["Users:User:Password"];

        if (login.Username == adminUsername && login.Password == adminPassword)
            return Ok(GenerateToken(login.Username, "Admin"));

        if (login.Username == userUsername && login.Password == userPassword)
            return Ok(GenerateToken(login.Username, "User"));

        return Unauthorized();
    }

    private object GenerateToken(string username, string role)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"]!)),
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