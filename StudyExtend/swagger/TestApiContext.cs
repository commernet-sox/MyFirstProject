using Microsoft.EntityFrameworkCore;

namespace swagger
{
    public class TestApiContext : DbContext
    {
        public TestApiContext(DbContextOptions<TestApiContext> options) : base(options)
        {

        }
    }
}
