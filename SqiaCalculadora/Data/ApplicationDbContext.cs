using Microsoft.EntityFrameworkCore;
using SqiaCalculadora.Models;

namespace SqiaCalculadora.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Cotacao> Cotacoes { get; set; } = null!;
}