using Entities;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class ToDoListDbContext : DbContext
{
    public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options) : base(options)
    {
    }

    public DbSet<ToDoListEntity> ToDoLists { get; set; } = null!;

    public DbSet<TaskEntity> Tasks { get; set; } = null!;

    public DbSet<CommentEntity> Comments { get; set; } = null!;

    public DbSet<TagEntity> Tags { get; set; } = null!;

    public DbSet<TaskTag> TaskTags { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ToDoListEntity>()
            .HasMany(tdl => tdl.Tasks)
            .WithOne(t => t.ToDoList)
            .HasForeignKey(t => t.ToDoListId);

        modelBuilder.Entity<TaskEntity>()
            .HasMany(t => t.Comments)
            .WithOne(c => c.Task)
            .HasForeignKey(c => c.TaskId);

        modelBuilder.Entity<TaskTag>()
            .HasKey(ttm=> new { ttm.TaskId, ttm.TagId });

        modelBuilder.Entity<TaskTag>()
            .HasOne(ttm => ttm.Task)
            .WithMany(t => t.TaskTags)
            .HasForeignKey(ttm => ttm.TaskId);

        modelBuilder.Entity<TaskTag>()
            .HasOne(t => t.Tag)
            .WithMany(tm => tm.TaskTags)
            .HasForeignKey(ttm => ttm.TagId);
    }
}
