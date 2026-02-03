using JobOffersManager.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JobOffersManager.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<JobOffer> JobOffers => Set<JobOffer>();
}
