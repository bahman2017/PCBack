using Microsoft.EntityFrameworkCore;
using PCBack.Models;

namespace PCBack.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<PatentAnalysis> PatentAnalyses => Set<PatentAnalysis>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PatentAnalysis>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.CreatedAt).HasColumnType("timestamp with time zone");
        });
    }
}
