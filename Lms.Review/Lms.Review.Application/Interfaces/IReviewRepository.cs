
using Review.Domain.Entities;

namespace Review.Application.Interfaces
{
    public interface IReviewRepository
    {
        Task AddAsync(ReviewEntity review, CancellationToken ct);
        Task<List<ReviewEntity>> GetByCourseIdAsync(int courseId, CancellationToken ct);
    }
}
