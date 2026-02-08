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
}
