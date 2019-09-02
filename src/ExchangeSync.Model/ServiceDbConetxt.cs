using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSync.Model
{
    public class ServiceDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(@"Data Source=(local)\SQLEXPRESS;Initial Catalog=ExchangeSync_Connect;Integrated Security=True;MultipleActiveResultSets=true");
        }

        public virtual DbSet<UserConnect> UserConnects { get; set; }

        public virtual DbSet<UserWeChat> UserWeChats { get; set; }
    }
}
