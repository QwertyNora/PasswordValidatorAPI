using Microsoft.EntityFrameworkCore;
using PasswordValidatorApi.Models;

namespace PasswordValidatorAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ValidationAttempt> ValidationAttempts { get; set; }
    }
}