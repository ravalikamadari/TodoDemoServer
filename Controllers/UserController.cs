using Todo.Models;
using Todo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Todo.Utilities;
using Todo.DTOs;

namespace Todo.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _user;
    private readonly IConfiguration _config;

    public UserController(ILogger<UserController> logger,
    IUserRepository user, IConfiguration config)
    {
        _logger = logger;
        _user = user;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResDTO>> Login(
        [FromBody] UserLoginDTO Data
    )
    {
        var existingUser = await _user.GetByUsername(Data.Username);

        if (existingUser is null)
            return NotFound();

        if (existingUser.Password != Data.Password)
            return BadRequest("Incorrect password");

        var token = Generate(existingUser);

        var res = new UserLoginResDTO
        {
            Id = existingUser.Id,
            Username = existingUser.Username,
            Token = token,
        };

        return Ok(res);
    }

    private string Generate(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(TodoConstants.Id, user.Id.ToString()),
            new Claim(TodoConstants.Username, user.Username),
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
