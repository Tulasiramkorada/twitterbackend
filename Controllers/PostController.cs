
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.DTOs;
using Twitter.Models;
using Twitter.Repositories;
using Twitter.Utilities;

namespace Twitter.Controllers;

[ApiController]
[Route("api/post")]
[Authorize]

public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly IPostRepository _post;

    public PostController(ILogger<PostController> logger,
    IPostRepository post)
    {
        _logger = logger;
        _post = post;
    }
    // private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    // {
    //     return Convert.ToInt32(claims.Where(x => x.Type == UserConstants.UserId).First().Value);
    // }
    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost([FromBody] PostCreateDTO Data)
    {
        var currentUserId = GetCurrentUserId();
        var PostCount = (await _post.GetPostByUserId(currentUserId)).Count;
        if (PostCount >= 5)

            return BadRequest("You can't create more than 5 posts");


        // var UserId = GetUserIdFromClaims(User.Claims);

        var toCreatePost = new Post
        {
            // PostId = Data.PostId,

            Title = Data.Title.Trim(),

            UserId = currentUserId,


        };
        var createdItem = await _post.Create(toCreatePost);

        return StatusCode(201, createdItem);
    }

    [HttpPut("{post_id}")]
    [Authorize]
    public async Task<ActionResult> UpdatePost([FromRoute] long post_id,
   [FromBody] PostUpdateDTO Data)
    {
        var existing = await _post.GetById(post_id);
        var currentuserId = GetCurrentUserId();

        // var existingItem = await _post.GetById(post_id);

        if (currentuserId != existing.UserId)
            return Unauthorized("you are not authorized to update this post");

        if (existing == null)
            return NotFound("Post not found");
        var toUpdatePost = existing with
        {
            Title = Data.Title is null ? existing.Title : Data.Title.Trim(),
            // IsComplete = !Data.IsComplete.HasValue ? existingItem.IsComplete : Data.IsComplete.Value,
        };
        var didUpdate = await _post.Update(toUpdatePost);
        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError);
        return Ok("Post updated");



    }
    [HttpDelete("{post_id}")]
    [Authorize]
    public async Task<ActionResult> DeletePost([FromRoute] long post_id)
    {
        var post = await _post.GetById(post_id);

        var currentUserid = GetCurrentUserId();

        if (currentUserid != post.UserId)
            return Unauthorized("You are Unauthorized to delete this post");

        if (post == null)
            return NotFound("post Not found");

        var didDelete = await _post.Delete(post_id);

        if (!didDelete)
            return StatusCode(500, "Internal Server Error");
        return Ok("Deleted");


    }
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<Post>>> GetAllPost()
    {
        var allPosts = await _post.GetAllPosts();
        return Ok(allPosts);
    }
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Post>> GetPost([FromRoute] long id)
    {
        var post = await _post.GetById(id);
        return Ok(post);
    }

    private long GetCurrentUserId()
    {
        var userClaims = User.Claims;
        return Int64.Parse(userClaims.FirstOrDefault(x => x.Type == UserConstants.UserId)?.Value);
    }
}