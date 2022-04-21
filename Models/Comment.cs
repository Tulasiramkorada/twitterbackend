namespace Twitter.Models;

public record Comment
{
    public long Id { get; set; }

    public string CommentText { get; set; }

    public long PostId { get; set; }

    public long UserId { get; set; }

    // public DateTimeOffset CreatedAt { get; set; }

    // public DateTimeOffset UpdatedAt { get; set; }
}