using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Twitter.DTOs;

public record PostDTO
{
    [JsonPropertyName("post_id")]

    public long PostId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("user_id")]

    public int user_id { get; set; }

    // [JsonPropertyName("updated_at")]
    // public DateTimeOffset UpdatedAt { get; set; }

    // [JsonPropertyName("created_at")]

    // public DateTimeOffset CreatedAt { get; set; }

}

public record PostCreateDTO
{
    // [JsonPropertyName("post_id")]
    // public long PostId { get; set; }

    [JsonPropertyName("title")]

    public string Title { get; set; }

    // [JsonPropertyName("user_id")]
    // public int UserId { get; set; }
}

public record PostUpdateDTO
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
}


