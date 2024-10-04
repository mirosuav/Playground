using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            using (var db = new UserContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var user = new User()
                {
                    Name = "Tom"
                };
                user.Roles = new[]
                {
                    new UserRole() { Role = "Manager", User = user },
                    new UserRole() { Role = "HR", User = user },
                };

                db.Users.Add(user);
                db.SaveChanges();
            }

            using (var db = new UserContext())
            {
                var user = db.Users.Include(x => x.Roles).FirstOrDefault();

                user!.Roles = new List<UserRole>()
                {
                        new UserRole() { Role = "Admin", User = user },
                        new UserRole() { Role = "User", User = user },
                        new UserRole() { Role = "Contributor", User = user },
                };

                db.SaveChanges();
            }

        }
    }
}


public class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public required string Name { get; set; }
    public virtual ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
}

[PrimaryKey(nameof(UserId), nameof(Role))]
public class UserRole
{
    public int UserId { get; set; }
    public virtual User? User { get; set; }

    [StringLength(50)]
    public required string Role { get; set; }
}


public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=test;Integrated security=True;Trust Server Certificate=True");
        optionsBuilder.LogTo(Console.WriteLine);
    }
}