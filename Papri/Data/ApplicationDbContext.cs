using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Papri.Models;

namespace Papri.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Headline> Headlines => Set<Headline>();
    public DbSet<Notice> Notices => Set<Notice>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<News> News => Set<News>();
    public DbSet<Slider> Sliders => Set<Slider>();
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<JobOpportunity> JobOpportunities => Set<JobOpportunity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Headline>().HasIndex(h => h.CreatedAt);
        builder.Entity<Notice>().HasIndex(n => n.Date);
        builder.Entity<Project>().HasIndex(p => p.Date);
        builder.Entity<News>().HasIndex(n => n.Date);
        builder.Entity<Slider>().HasIndex(s => s.DisplayOrder);
        builder.Entity<JobOpportunity>().HasIndex(j => j.Deadline);
    }
}
