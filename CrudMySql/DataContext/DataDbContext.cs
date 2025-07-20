using CrudMySql.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace CrudMySql.DataContext
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Enable auto-inclusion of navigation property
            modelBuilder.Entity<Employee>()
                .Navigation(e => e.IDepartment)
                .AutoInclude();

            // Department entity config
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.DepName)
                      .HasMaxLength(50)
                      .IsUnicode(false);
            });

            // Employee entity config
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(e => e.Email)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(e => e.Salary)
                      .HasColumnType("decimal(10,2)");

                entity.Property(e => e.isActive)
                      .HasDefaultValue(true)
                      .IsRequired();

                entity.HasQueryFilter(e => e.isActive);

                entity.Property(e => e.CreatedDate)
                     .HasDefaultValueSql("GETUTCDATE()") // Ensures DB defaulting
                     .IsRequired();

                entity.HasOne(e => e.IDepartment)
                      .WithMany(d => d.Employees)
                      .HasForeignKey(e => e.DepartmentId)
                      .HasConstraintName("FK_Employee_Department");
            });
        }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();
    }
}
