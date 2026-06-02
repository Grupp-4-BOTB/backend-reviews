using Review.Application.Dtos;
using Review.Application.DTOs;
using Review.Application.Interfaces;
using Review.Domain.Entities;

namespace Review.Application.Services;

public class ReviewService(IReviewRepository reviewRepository) : IReviewService
{

    private readonly IReviewRepository _reviewRepository = reviewRepository;

    public async Task CreateReviewAsync(int courseId, CreateReviewDto reviewDto, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(reviewDto.Comment))
        {
            throw new ArgumentException("Comment cannot be empty.");
        }
        var review = new ReviewEntity
        {
            CourseId = courseId,
            StudentId = reviewDto.StudentId,
            Comment = reviewDto.Comment,
            CreatedAt = DateTime.UtcNow
        };
        await _reviewRepository.AddAsync(review, ct);
    }

    public async Task<List<ReviewDto>> GetReviewsByCourseIdAsync(int courseId, CancellationToken ct)
    {
        var reviews = await _reviewRepository.GetByCourseIdAsync(courseId, ct);

        if(reviews == null) { 
            return [];
        }

        var reviewDtos = reviews.Select(r => new ReviewDto
        {
            Id = r.Id,
            CourseId = r.CourseId,
            StudentId = r.StudentId,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt
        }).ToList();

        return reviewDtos;
    }
}
