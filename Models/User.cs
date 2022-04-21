using Twitter.DTOs;

namespace Twitter.Models;

public record User
{
    public long UserId { get; set; }

    public string FullName { get; set; }

    public string Password { get; set; }
    public string Email { get; set; }


    public UserLoginDTO asDTO => new UserLoginDTO
    {

        Password = Password,
        Email = Email
    };
}


