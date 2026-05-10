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
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<DonorPartner> DonorPartners => Set<DonorPartner>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<AppreciativeStory> AppreciativeStories => Set<AppreciativeStory>();
    public DbSet<LegendaryVisitor> LegendaryVisitors => Set<LegendaryVisitor>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Headline>().HasIndex(h => h.CreatedAt);
        builder.Entity<Notice>().HasIndex(n => n.Date);
        builder.Entity<Project>().HasIndex(p => p.Date);
        builder.Entity<News>().HasIndex(n => n.Date);
        builder.Entity<Slider>().HasIndex(s => s.DisplayOrder);
        builder.Entity<JobOpportunity>().HasIndex(j => j.Deadline);
        builder.Entity<Feedback>().HasIndex(f => f.SubmittedAt);
        builder.Entity<DonorPartner>().HasIndex(d => new { d.Category, d.DisplayOrder });
        builder.Entity<Experience>().HasIndex(e => e.DisplayOrder);
        builder.Entity<AppreciativeStory>().HasIndex(s => s.DisplayOrder);
        builder.Entity<LegendaryVisitor>().HasIndex(v => v.DisplayOrder);
    }
}
