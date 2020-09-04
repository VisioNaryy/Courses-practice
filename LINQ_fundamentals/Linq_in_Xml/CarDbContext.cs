using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Linq_to_Xml
{
    public class CarDbContext: DbContext
    {
        public CarDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=helloappdb;Trusted_Connection=True;");
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }
        public DbSet<Car> Cars { get; set; }
        //Logger factory
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(new MyLoggerProvider());    // указываем наш провайдер логгирования
        });
    }
}
