using Microsoft.EntityFrameworkCore;
namespace OrchestratedSagaApp
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsLetterOnBoardSagaData>().HasKey(x=>x.CorrelationId);
        }
        public virtual DbSet<WeatherForecast> WeatherForecast { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }

        public virtual DbSet<NewsLetterOnBoardSagaData> NewsLetterOnBoardSagaData { get; set; }

    }
}

