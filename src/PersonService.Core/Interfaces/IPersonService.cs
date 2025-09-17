using PersonService.Core.Models;

namespace PersonService.Core.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с сущностью Person.
/// </summary>
public interface IPersonService
{
    public Task<Person> CreatePersonAsync(string name, int? age, string? address, string? work);
    
    public Task<Person> UpdatePersonAsync(int id, string name, int? age, string? address, string? work);
    
    public Task<Person> GetPersonByIdAsync(int id);
    
    public Task<Person> DeletePersonAsync(int id);
    
    public Task<List<Person>> GetPeopleAsync();
}