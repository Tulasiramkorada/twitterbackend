using Twitter.Models;
using Twitter.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Twitter.Utilities;
using Twitter.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Twitter.Controllers;

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
    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserCreateDTO Data)
    {


        var toCreateUser = new User
        {
            FullName = Data.FullName.Trim(),

            Password = BCrypt.Net.BCrypt.HashPassword(Data.Password.Trim()),

            Email = Data.Email.Trim()
        };
        var createdUser = await _user.Create(toCreateUser);

        return StatusCode(StatusCodes.Status201Created, createdUser.asDTO);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResDTO>> Login(
            [FromBody] UserLoginDTO Data
        )
    {
        var existingUser = await _user.GetUser(Data.Email);

        if (existingUser is null)
            return NotFound("User Not Found With  Email Address");


        if (!BCrypt.Net.BCrypt.Verify(Data.Password, existingUser.Password))

            return BadRequest("Incorrect password");

        var token = Generate(existingUser);

        var res = new UserLoginResDTO
        {
            UserId = existingUser.UserId,
            FullName = existingUser.FullName,
            Email = existingUser.Email,
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
            new Claim(UserConstants.UserId, user.UserId.ToString()),
            new Claim(UserConstants.FullName, user.FullName),
            new Claim(UserConstants.Email, user.Email),
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                   _config["Jwt:Audience"],
                   claims,
                   expires: DateTime.Now.AddMinutes(60),
                   signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == UserConstants.UserId).First().Value);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> UpdateUser([FromBody] UserUpdateDTO Data)
    {
        // var UserId = GetUserIdFromClaims(User.Claims);
        // var existing = await _user.GetById(UserId);
        // if (existing is null)
        //     return NotFound("No User found with given User Id");
        // if (existing.UserId != UserId)
        //     return StatusCode(403, "You are not authorized to update this user");
        // var toUpdateUser = existing with
        var currentUserId = GetCurrentUserId();
        var toUpdateUser = new User
        {
            UserId = currentUserId,
            FullName = Data.FullName.Trim(),
        };

        var didUpdate = await _user.Update(toUpdateUser);
        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update user");

        return Ok("Updated");
    }
    private long GetCurrentUserId()
    {
        var userClaims = User.Claims;
        return Int64.Parse(userClaims.FirstOrDefault(x => x.Type == UserConstants.UserId)?.Value);
    }
}



