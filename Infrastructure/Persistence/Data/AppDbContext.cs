using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Task = Domain.Models.Task;

namespace Infrastructure.Persistence.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Task> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            modelBuilder.Entity(entityType.Name).ToTable(entityType.ClrType.Name);
        }
        
        // Настройка User -> Role (Many-to-One)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict); // или Cascade, в зависимости от требований

        // Настройка User -> RefreshToken (One-to-Many)
        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithOne(u => u.Token)
            .OnDelete(DeleteBehavior.Cascade); // Удаление токенов при удалении пользователя

        // Настройка User -> Task (One-to-Many)
        modelBuilder.Entity<Task>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Удаление задач при удалении пользователя

        // Настройка индексов
        modelBuilder.Entity<RefreshToken>()
            .HasIndex(rt => rt.Token)
            .IsUnique();

        modelBuilder.Entity<RefreshToken>()
            .HasIndex(rt => rt.UserId);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Task>()
            .HasIndex(t => t.UserId);

        // Настройка enum преобразования для Status
        modelBuilder.Entity<Task>()
            .Property(t => t.Status)
            .HasConversion<string>(); // или .HasConversion<int>(), в зависимости от предпочтений

        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = 1,
                Name = "User",
            }
        );
    }
}