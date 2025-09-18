using PersonService.Core.Models;

namespace PersonService.Tests.Services.Helpers;

public static class PersonMother
{
    public static Person CreatePerson(int id = 1,
        string name = "Aleksey",
        int? age = null,
        string? address = null,
        string? work = null)
    {
        return new Person(id, name, age, address, work);
    }
}