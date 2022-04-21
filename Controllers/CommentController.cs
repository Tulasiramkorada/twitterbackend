using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.DTOs;
using Twitter.Models;
using Twitter.Repositories;
using Twitter.Utilities;

namespace Twitter.Controllers;

[ApiController]
[Authorize]
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentRepository _comment;

    public CommentController(ILogger<CommentController> logger,
    ICommentRepository comment)
    {
        _logger = logger;
        _comment = comment;
    }
    [HttpPost("{post_id}")]
    public async Task<ActionResult<Comment>> CreateComment([FromRoute] int post_id, [FromBody] CommentCreateDTO Data)
    {
        var currentUserId = GetCurrentUserId();
        int PostId = post_id;

        var toCreateItem = new Comment
        {
            CommentText = Data.CommentText.Trim(),
            PostId = PostId,
            UserId = currentUserId,
        };

        var createdItem = await _comment.Create(toCreateItem);

        return StatusCode(201, createdItem);
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComment([FromRoute] long id)
    {
        var currentUserId = GetCurrentUserId();

        var existingItem = await _comment.GetById(id);
        if (currentUserId != existingItem.UserId)

            return Unauthorized("You are not authorized to delete this comment");



        if (existingItem is null)
            return NotFound("Comment not found");

        // if (existingItem.UserId != userId)
        //     return StatusCode(403, "You cannot delete other's Post");

        var didDelete = await _comment.Delete(id);
        if (!didDelete)
            return StatusCode(500, "Something went wrong");


        return Ok("Deleted");
    }

    [HttpGet("{post_id}")]
    public async Task<ActionResult<List<Comment>>> GetAllComments([FromRoute] int post_id)
    {
        var allComments = await _comment.GetAllComments(post_id);
        return Ok(allComments);
    }
    private long GetCurrentUserId()
    {
        var userClaims = User.Claims;
        return Int64.Parse(userClaims.FirstOrDefault(x => x.Type == UserConstants.UserId)?.Value);
    }
}

