using Microsoft.EntityFrameworkCore;
using PersonService.Database.Context.Configurations;
using PersonService.Database.Models;

namespace PersonService.Database.Context;

public class PersonServiceContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    
    public PersonServiceContext(DbContextOptions<PersonServiceContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
    }
}