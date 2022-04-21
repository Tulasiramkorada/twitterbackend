using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Twitter.DTOs;

public record UserDTO
{
    [JsonPropertyName("user_id")]

    public long UserId { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("password")]

    public string Password { get; set; }

    [JsonPropertyName("password")]
    public string Email { get; set; }

}

public record UserCreateDTO
{
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("password")]

    public string Password { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }
}

public record UserUpdateDTO
{
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }
}


public record UserLoginDTO
{
    [Required]
    [JsonPropertyName("email")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    [JsonPropertyName("password")]
    [MaxLength(255)]
    public string Password { get; set; }
}

public record UserLoginResDTO
{
    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("user_id")]

    public long UserId { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }
}