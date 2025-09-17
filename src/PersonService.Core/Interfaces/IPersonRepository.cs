using PersonService.Core.Models;

namespace PersonService.Core.Interfaces;

/// <summary>
/// Интерфейс репозитория для работы с сущностью Person.
/// </summary>
public interface IPersonRepository
{
    public Task CreatePerson(Person person);
}