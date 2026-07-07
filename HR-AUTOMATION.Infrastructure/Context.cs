
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HR_AUTOMATION.Infrastructure
{
    public class Context : DbContext
    {
        public Context()
        {

        }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationBuilder cb = new();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            cb.AddJsonFile(path, false);

            IConfigurationRoot root = cb.Build();
            string connectionString = root.GetValue<string>("ConnectionString")!;
            optionsBuilder.UseNpgsql(connectionString);

            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            base.OnModelCreating(modelBuilder);
        }

    }
}
