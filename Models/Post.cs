namespace Twitter.Models;

public record Post
{
    public long PostId { get; set; }

    public string Title { get; set; }

    public string UpdatedAt { get; set; }

    public long UserId { get; set; }



    public string CreatedAt { get; set; }
}