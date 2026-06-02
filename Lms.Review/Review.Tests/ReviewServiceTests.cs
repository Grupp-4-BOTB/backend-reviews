

using Moq;
using Review.Application.Dtos;
using Review.Application.DTOs;
using Review.Application.Interfaces;
using Review.Application.Services;
using Review.Domain.Entities;

namespace Review.Tests;

public class ReviewServiceTests
{
    [Fact]
    public async Task CreateReviewAsync_Should_Add_Review_When_Data_Is_Valid()
    {
        // Arrange
        var repositoryMock = new Mock<IReviewRepository>();
        var service = new ReviewService(repositoryMock.Object);

        var dto = new CreateReviewDto
        {
            StudentId = 1,
            Comment = "Great course!"
        };

        // Act
        await service.CreateReviewAsync(3, dto, CancellationToken.None);

        // Assert
        repositoryMock.Verify(repo => repo.AddAsync(
            It.Is<ReviewEntity>(r =>
                r.CourseId == 3 &&
                r.StudentId == 1 &&
                r.Comment == "Great course!"),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateReviewAsync_Should_Throw_When_Comment_Is_Empty()
    {
        // Arrange
        var repositoryMock = new Mock<IReviewRepository>();
        var service = new ReviewService(repositoryMock.Object);

        var dto = new CreateReviewDto
        {
            StudentId = 1,
            Comment = ""
        };

        // Act
        var action = async () =>
            await service.CreateReviewAsync(3, dto, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(action);

        repositoryMock.Verify(repo => repo.AddAsync(
            It.IsAny<ReviewEntity>(),
            It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateReviewAsync_Should_Throw_When_Comment_Is_Only_Whitespace()
    {
        // Arrange
        var repositoryMock = new Mock<IReviewRepository>();
        var service = new ReviewService(repositoryMock.Object);

        var dto = new CreateReviewDto
        {
            StudentId = 1,
            Comment = "     "
        };

        // Act
        var action = async () =>
            await service.CreateReviewAsync(3, dto, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(action);

        repositoryMock.Verify(repo => repo.AddAsync(
            It.IsAny<ReviewEntity>(),
            It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetReviewsByCourseIdAsync_Should_Return_Reviews_For_Course()
    {
        // Arrange
        var repositoryMock = new Mock<IReviewRepository>();

        repositoryMock
            .Setup(repo => repo.GetByCourseIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ReviewEntity>
            {
                new ReviewEntity
                {
                    Id = 1,
                    CourseId = 3,
                    StudentId = 1,
                    Comment = "Good course",
                    CreatedAt = DateTime.UtcNow
                }
            });

        var service = new ReviewService(repositoryMock.Object);

        // Act
        var result = await service.GetReviewsByCourseIdAsync(3, CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal(3, result[0].CourseId);
        Assert.Equal("Good course", result[0].Comment);
    }

    [Fact]
    public async Task GetReviewsByCourseIdAsync_Should_Return_Empty_List_When_No_Reviews_Exist()
    {
        // Arrange
        var repositoryMock = new Mock<IReviewRepository>();

        repositoryMock
            .Setup(repo => repo.GetByCourseIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ReviewEntity>());

        var service = new ReviewService(repositoryMock.Object);

        // Act
        var result = await service.GetReviewsByCourseIdAsync(99, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}