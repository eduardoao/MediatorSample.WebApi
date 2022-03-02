using MediatorSample.WebApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MediatorSample.WebApi.Infrastructure.Context
{
    public interface IApplicationContext
    {
        DbSet<Product> Products { get; set; }

        Task<int> SaveChangesAsync();
    }
}