using DbPerson = PersonService.Database.Models.Person;
using CorePerson = PersonService.Core.Models.Person;

namespace PersonService.Database.Repositories.Converters;

public static class PersonConverter
{
    public static CorePerson Convert(DbPerson person)
    {
        return new CorePerson(person.Id,
            person.Name,
            person.Age,
            person.Address,
            person.Work);
    }
}