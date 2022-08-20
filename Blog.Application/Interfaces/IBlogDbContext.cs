using Blog.Domain.Models;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Interfaces
{
    public interface IBlogDbContext
    {
        DbSet<Article> Articles { get; set; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
