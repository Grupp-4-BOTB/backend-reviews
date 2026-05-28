using Microsoft.AspNetCore.Mvc;
using Review.Application.Dtos;
using Review.Application.DTOs;
using Review.Application.Interfaces;

namespace Review.Api.Controllers;

[ApiController]
[Route("api/courses/{courseId:int}/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReviewDto>>> GetReviews(int courseId, CancellationToken ct)
    {
        var reviews = await _reviewService.GetReviewsByCourseIdAsync(courseId, ct);

        return Ok(reviews);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReview(int courseId, CreateReviewDto dto, CancellationToken ct)
    {
        try
        {
            await _reviewService.CreateReviewAsync(courseId, dto, ct);

            return Created();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
