using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeSync.Model.EnterpiseContactModel;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSync.Model
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext()
        {

        }

        public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(@"Server=db-01.server.scrbg.com,1433;Database=ITSCRBG_Contact;User Id=base;Password=MS@scrbg2016;MultipleActiveResultSets=true");
        }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeePosition> EmployeePositions { get; set; }

        public DbSet<EmployeeAuth> EmployeeAuths { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .HasIndex(o => o.Name);
            modelBuilder.Entity<Employee>()
               .HasIndex(o => o.Number);
            modelBuilder.Entity<Employee>()
              .HasIndex(o => o.UserId);
            modelBuilder.Entity<Employee>()
              .HasIndex(o => o.UserName);
            modelBuilder.Entity<Employee>()
              .HasIndex(o => o.IdCardNo);
            modelBuilder.Entity<Employee>()
             .HasIndex(o => o.PrimaryDepartmentId);
            modelBuilder.Entity<EmployeePosition>()
                .HasKey(o => new { o.EmployeeId, o.PositionId });

        }
    }
}
