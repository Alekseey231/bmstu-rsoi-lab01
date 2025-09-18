using Microsoft.Extensions.Logging;
using PersonService.Core.Interfaces;
using PersonService.Core.Models;

namespace PersonService.Services.PersonService;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<PersonService> _logger;

    public PersonService(IPersonRepository personRepository, ILogger<PersonService> logger)
    {
        _personRepository = personRepository;
        _logger = logger;
    }
    
    public async Task<Person> CreatePersonAsync(string name, int? age, string? address, string? work)
    {
        _logger.LogDebug("Creating person");
        
        var person = await _personRepository.CreatePersonAsync(name, age, address, work);
        
        _logger.LogInformation("Create person with id {Id}", person.Id);
        
        return person;
    }

    public async Task<Person> UpdatePersonAsync(int id, string name, int? age, string? address, string? work)
    {
        _logger.LogDebug("Updating person");
        
        var person = await _personRepository.UpdatePersonAsync(id, name, age, address, work);
        
        _logger.LogInformation("Update person with id {Id}", person.Id);
        
        return person;
    }

    public Task<Person> GetPersonByIdAsync(int id)
    {
        return _personRepository.GetPersonByIdAsync(id);
    }

    public Task<Person> DeletePersonAsync(int id)
    {
        _logger.LogDebug("Deleting person");

        var person = _personRepository.DeletePersonAsync(id);
        
        _logger.LogInformation("Delete person with id {Id}", person.Id);
        
        return person;
    }

    public Task<List<Person>> GetPeopleAsync()
    {
        return _personRepository.GetPeopleAsync();
    }
}