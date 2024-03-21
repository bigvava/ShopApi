using Microsoft.EntityFrameworkCore;

namespace ShopApi.Entities
{

    public class ShopContext : DbContext
    {
    public ShopContext(DbContextOptions<ShopContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UsersRole> UsersRoles  { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasKey(x=>x.Id);
        modelBuilder.Entity<Category>().Property(x=>x.Name).IsRequired().HasColumnType("nvarchar(150)");
        modelBuilder.Entity<Product>().HasOne(x=>x.Category).WithMany(x=>x.Products).HasForeignKey(x=>x.CategoryId);
        modelBuilder.Entity<Product>().Property(x => x.Price).HasPrecision(10, 2);

        modelBuilder.Entity<User>().HasKey(x => x.Id);
        modelBuilder.Entity<Role>().HasKey(x => x.Id);

        modelBuilder.Entity<UsersRole>().HasOne(x => x.User).WithMany(x => x.UsersRoles).HasForeignKey(x=>x.UserId);
        modelBuilder.Entity<UsersRole>().HasOne(x => x.Role).WithMany(x => x.UsersRoles).HasForeignKey(x => x.RoleID);
        modelBuilder.Entity<UsersRole>().HasKey(t => new { t.RoleID, t.UserId });




        }

    }
}
