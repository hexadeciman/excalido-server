using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistance;

using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<ListEntity> Lists { get; set; }
    public DbSet<Todo> Todos { get; set; }
    public DbSet<BoardAudit> BoardAudits { get; set; }
    public DbSet<ShareEntity> EntityShares { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<EntityType> EntityTypes { get; set; }
    private readonly IConfiguration _configuration;


    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Enums to Map to PostgreSQL Enums
        modelBuilder.HasPostgresEnum<RoleEnum>();
        modelBuilder.HasPostgresEnum<TodoStatusEnum>();
        modelBuilder.HasPostgresEnum<ActionEnum>();

        // ────────────────────────────────────────────────────────────────────────────────
        // ** USER & ROLES RELATIONSHIPS **
        // ────────────────────────────────────────────────────────────────────────────────

        modelBuilder.Entity<User>()
            .HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.PermissionId }); // Composite Primary Key

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Permission)
            .WithMany(p => p.UserRoles)
            .HasForeignKey(ur => ur.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

        // ────────────────────────────────────────────────────────────────────────────────
        // ** BOARD & LIST RELATIONSHIPS **
        // ────────────────────────────────────────────────────────────────────────────────

        modelBuilder.Entity<Board>()
            .HasOne(b => b.Owner)
            .WithMany()
            .HasForeignKey(b => b.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ListEntity>()
            .HasOne(l => l.Board)
            .WithMany(b => b.Lists)
            .HasForeignKey(l => l.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        // ────────────────────────────────────────────────────────────────────────────────
        // ** TODO RELATIONSHIPS **
        // ────────────────────────────────────────────────────────────────────────────────

        modelBuilder.Entity<Todo>()
            .HasOne(t => t.List)
            .WithMany(l => l.Todos)
            .HasForeignKey(t => t.ListId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Todo>()
            .HasOne(t => t.ParentTodo)
            .WithMany(t => t.SubTodos)
            .HasForeignKey(t => t.ParentTodoId)
            .OnDelete(DeleteBehavior.Cascade); // Allow Sub-Tasks

        // ────────────────────────────────────────────────────────────────────────────────
        // ** BOARD AUDIT RELATIONSHIPS **
        // ────────────────────────────────────────────────────────────────────────────────

        modelBuilder.Entity<BoardAudit>()
            .HasOne(ba => ba.Board)
            .WithMany(b => b.AuditLogs)
            .HasForeignKey(ba => ba.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardAudit>()
            .HasOne(ba => ba.User)
            .WithMany()
            .HasForeignKey(ba => ba.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of users with history logs

        // ────────────────────────────────────────────────────────────────────────────────
        // ** ENTITY SHARING (GENERIC SHARING FOR LISTS, BOARDS, TODOS, ETC.) **
        // ────────────────────────────────────────────────────────────────────────────────

        modelBuilder.Entity<ShareEntity>()
            .HasOne(es => es.User)
            .WithMany(u => u.SharedEntities)
            .HasForeignKey(es => es.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ShareEntity>()
            .HasOne(es => es.Permission)
            .WithMany()
            .HasForeignKey(es => es.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<ShareEntity>()
            .HasOne(es => es.EntityType)
            .WithMany(et => et.SharedEntities)
            .HasForeignKey(es => es.EntityTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        // ────────────────────────────────────────────────────────────────────────────────
        // ** INDEXES FOR PERFORMANCE **
        // ────────────────────────────────────────────────────────────────────────────────

        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<ShareEntity>().HasIndex(es => new { es.EntityId, es.EntityTypeId });
        modelBuilder.Entity<BoardAudit>().HasIndex(ba => ba.CreatedAt);
    }
}
