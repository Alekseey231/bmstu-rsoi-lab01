using Microsoft.EntityFrameworkCore;
using PersonService.Core.Exceptions;
using PersonService.Core.Interfaces;
using PersonService.Core.Models;
using PersonService.Database.Context;
using PersonService.Database.Repositories.Converters;
using DbPerson = PersonService.Database.Models.Person;

namespace PersonService.Database.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly PersonServiceContext _context;

    public PersonRepository(PersonServiceContext context)
    {
        _context = context;
    }
    
    public async Task<Person> CreatePersonAsync(string name, int? age, string? address, string? work)
    {
        var dbPerson = new DbPerson(name, age, address, work);
        
        await _context.Persons.AddAsync(dbPerson);
        await _context.SaveChangesAsync();
        
        return PersonConverter.Convert(dbPerson);
    }

    public async Task<Person> UpdatePersonAsync(int id, string name, int? age, string? address, string? work)
    {
        var dbPerson = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
        if (dbPerson is null)
            throw new PersonNotFoundException($"Person with id {id} was not found");
        
        dbPerson.Name = name;
        dbPerson.Age = age;
        dbPerson.Address = address;
        dbPerson.Work = work;
        
        await _context.SaveChangesAsync();
        
        return PersonConverter.Convert(dbPerson);
    }

    public async Task<Person> GetPersonByIdAsync(int id)
    {
        var dbPerson = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
        if (dbPerson is null)
            throw new PersonNotFoundException($"Person with id {id} was not found");
        
        return PersonConverter.Convert(dbPerson);
    }

    public async Task<Person> DeletePersonAsync(int id)
    {
        var dbPerson = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
        if (dbPerson is null)
            throw new PersonNotFoundException($"Person with id {id} was not found");
        
        _context.Persons.Remove(dbPerson);
        await _context.SaveChangesAsync();
        
        return PersonConverter.Convert(dbPerson);
    }

    public async Task<List<Person>> GetPeopleAsync()
    {
        var persons = await _context.Persons.ToListAsync();
        
        return persons.ConvertAll(PersonConverter.Convert);
    }
}