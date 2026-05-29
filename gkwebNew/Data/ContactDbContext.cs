using gkweb.api.types.models;
using Microsoft.EntityFrameworkCore;

namespace gkwebNew.Data;

public class ContactDbContext(DbContextOptions<ContactDbContext> options) : DbContext(options)
{
    public DbSet<ContactSubmission> ContactSubmissions => Set<ContactSubmission>();
}
