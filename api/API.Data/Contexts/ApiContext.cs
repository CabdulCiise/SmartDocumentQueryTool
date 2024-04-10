using API.Data.Entities;
using API.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Data.Contexts;

public class ApiContext : DbContext
{
    private readonly IConfiguration Configuration;

    public ApiContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(Configuration.GetConnectionString("ApiContext"));
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Seed();
    }
}
