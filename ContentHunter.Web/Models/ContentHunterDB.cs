using System.Data.Entity;

namespace ContentHunter.Web.Models
{
    public class ContentHunterDB: DbContext
    {
        public DbSet<Instruction> Instructions{ get; set; }
        public DbSet<CrawlerResult> CrawlerResults { get; set; }
    }
}