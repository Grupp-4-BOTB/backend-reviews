using Review.Application.Dtos;
using Review.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Review.Application.Interfaces
{
    public interface IReviewService
    {
        Task CreateReviewAsync(int courseId, CreateReviewDto reviewDto, CancellationToken ct);
        Task<List<ReviewDto>> GetReviewsByCourseIdAsync(int courseId, CancellationToken ct);
    }
}
