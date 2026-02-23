using JobOffersManager.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobOffersManager.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<JobOffer> JobOffers => Set<JobOffer>();
}