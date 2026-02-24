using JobOffersManager.API.Entities;
using JobOffersManager.API.Services;
using JobOffersManager.Shared;
using JobOffersManager.Tests.Helpers;

namespace JobOffersManager.Tests.Services;

public class JobOffersServiceTests
{
    private JobOffersService CreateServiceWithSeedData()
    {
        var context = TestDbContextFactory.Create();

        context.JobOffers.AddRange(
            new JobOffer
            {
                Id = 1,
                Title = "Junior .NET",
                Location = "Gliwice",
                Seniority = "Junior",
                Description = "Desc1",
                Requirements = "Req1",
                Company = "Company1",
                Email = "junior@example.com",
                Salary = 1000,
                Country = "PL",
                Telephone = "123456789",
                Created = DateTime.UtcNow
            },
            new JobOffer
            {
                Id = 2,
                Title = "Senior .NET",
                Location = "Katowice",
                Seniority = "Senior",
                Description = "Desc2",
                Requirements = "Req2",
                Company = "Company2",
                Email = "senior@example.com",
                Salary = 2000,
                Country = "PL",
                Telephone = "987654321",
                Created = DateTime.UtcNow
            }
        );

        context.SaveChanges();

        return new JobOffersService(context);
    }

    [Fact]
    public async Task GetById_ShouldReturnCorrectJob()
    {
        var service = CreateServiceWithSeedData();
        var result = await service.GetByIdAsync(1);
        Assert.NotNull(result);
        Assert.Equal("Junior .NET", result!.Title);
    }

    [Fact]
    public async Task Create_ShouldAddJob()
    {
        var context = TestDbContextFactory.Create();
        var service = new JobOffersService(context);

        var dto = new CreateJobOfferDto
        {
            Title = "Test",
            Location = "City",
            Seniority = "Mid",
            Description = "Desc",
            Requirements = "Req",
            Company = "Comp",
            Email = "test@example.com",
            Salary = 1000,
            Country = "PL",
            Telephone = "123456789"
        };

        var result = await service.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("Test", result.Title);
        Assert.Single(context.JobOffers);
    }

    [Fact]
    public async Task Update_ShouldModifyExistingJob()
    {
        var service = CreateServiceWithSeedData();

        var updateDto = new UpdateJobOfferDto
        {
            Title = "Updated",
            Location = "UpdatedCity",
            Seniority = "Lead",
            Description = "UpdatedDesc",
            Requirements = "UpdatedReq",
            Company = "UpdatedCompany",
            Email = "updated@example.com",
            Salary = 3000,
            Country = "PL",
            Telephone = "111222333"
        };

        var result = await service.UpdateAsync(1, updateDto);

        Assert.NotNull(result);
        Assert.Equal("Updated", result!.Title);
        Assert.Equal("UpdatedCity", result.Location);
    }

    [Fact]
    public async Task Delete_ShouldRemoveJob()
    {
        var service = CreateServiceWithSeedData();

        var result = await service.DeleteAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task GetAll_ShouldFilterByLocation()
    {
        var service = CreateServiceWithSeedData();

        var query = new JobOfferQueryDto
        {
            Location = "Gliwice",
            Page = 1,
            PageSize = 10
        };

        var result = await service.GetAllAsync(query);

        Assert.Single(result.Items);
        Assert.Equal(1, result.TotalCount);
    }

    [Fact]
    public async Task GetAll_ShouldPaginateCorrectly()
    {
        var service = CreateServiceWithSeedData();

        var query = new JobOfferQueryDto
        {
            Page = 1,
            PageSize = 1
        };

        var result = await service.GetAllAsync(query);

        Assert.Equal(2, result.TotalCount);
        Assert.Single(result.Items);
    }
}