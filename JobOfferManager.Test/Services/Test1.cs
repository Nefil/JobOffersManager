using JobOffersManager.API.Data;
using JobOffersManager.API.Entities;
using JobOffersManager.API.Services;
using JobOffersManager.Shared;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JobOffersManager.Tests.Services;

public class JobOffersServiceTests
{
    private JobOffersService CreateService()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        context.JobOffers.AddRange(
            new JobOffer
            {
                Title = "Junior .NET",
                Location = "Gliwice",
                Seniority = "Junior",
                Created = DateTime.UtcNow
            },
            new JobOffer
            {
                Title = "Senior .NET",
                Location = "Katowice",
                Seniority = "Senior",
                Created = DateTime.UtcNow
            }
        );

        context.SaveChanges();

        return new JobOffersService(context);
    }

    [Fact]
    public void GetAll_ShouldReturnItemsAndTotalCount()
    {
        // arrange
        var service = CreateService();
        var query = new JobOfferQueryDto
        {
            Page = 1,
            PageSize = 10
        };

        // act
        var result = service.GetAll(query);

        // assert
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.Items.Count);
    }

    [Fact]
    public void GetAll_WithLocationFilter_ShouldReturnOnlyMatchingOffers()
    {
        // arrange
        var service = CreateService();
        var query = new JobOfferQueryDto
        {
            Location = "gli",
            Page = 1,
            PageSize = 10
        };

        // act
        var result = service.GetAll(query);

        // assert
        Assert.Single(result.Items);
        Assert.Equal("Gliwice", result.Items[0].Location);
    }

    [Fact]
    public void GetAll_SortedByTitleAscending_ShouldReturnSortedResults()
    {
        // arrange
        var service = CreateService();
        var query = new JobOfferQueryDto
        {
            SortBy = "title",
            SortOrder = "asc",
            Page = 1,
            PageSize = 10
        };

        // act
        var result = service.GetAll(query);

        // assert
        Assert.Equal("Junior .NET", result.Items[0].Title);
        Assert.Equal("Senior .NET", result.Items[1].Title);
    }

    [Fact]
    public void GetAll_WithPagination_ShouldReturnCorrectPage()
    {
        // arrange
        var service = CreateService();
        var query = new JobOfferQueryDto
        {
            Page = 2,
            PageSize = 1
        };

        // act
        var result = service.GetAll(query);

        // assert
        Assert.Single(result.Items);
        Assert.Equal(2, result.TotalCount);
    }
}
