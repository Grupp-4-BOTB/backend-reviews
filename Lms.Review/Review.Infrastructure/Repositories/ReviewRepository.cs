using Review.Application.Interfaces;
using Review.Domain.Entities;
using Review.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Review.Infrastructure.Repositories;

public class ReviewRepository(ReviewDbContext dbContext) : IReviewRepository
{
    private readonly ReviewDbContext _dbContext = dbContext;


    public async Task AddAsync(ReviewEntity review, CancellationToken ct)
    {
        _dbContext.Add(review);

        await _dbContext.SaveChangesAsync(ct);
    }


    public async Task<List<ReviewEntity>> GetByCourseIdAsync(int courseId, CancellationToken ct)
    {
        return await _dbContext.Reviews.Where(r => r.CourseId == courseId).OrderByDescending(r => r.CreatedAt).ToListAsync(ct);
    }
}
