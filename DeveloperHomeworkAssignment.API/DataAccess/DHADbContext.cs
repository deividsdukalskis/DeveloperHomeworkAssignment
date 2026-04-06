namespace DeveloperHomeworkAssignment.API.DataAccess
{
    using Microsoft.EntityFrameworkCore;
    using Entities;

    public class DHADbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DHADbContext(DbContextOptions<DHADbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<Profiles> Profiles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
            base.OnConfiguring(optionsBuilder);
        }
    }
}
