using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonService.Database.Models;

namespace PersonService.Database.Context.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasIndex(person => person.Id).IsUnique();
        builder.HasKey(person => person.Id);

        builder.Property(person => person.Id)
            .IsRequired();

        builder.Property(person => person.Name).IsRequired();
    }
}