using Microsoft.EntityFrameworkCore;
using RobotXTest.DataAccess.Core.Models;

namespace RobotXTest.DataAccess.DbContexts
{
    public class RobotXTestContext : DbContext
    {
        public RobotXTestContext(DbContextOptions<RobotXTestContext> options) : base(options)
        {
        }

        public DbSet<ClientRto> Clients { get; set; }
    }
}
