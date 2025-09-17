using CorePerson = PersonService.Core.Models.Person;
using DtoPerson = PersonService.Dto.Http.Models.Person;

namespace PersonService.Dto.Http.Converters;

public static class PersonConverter
{
    public static DtoPerson Convert(CorePerson model)
    {
        return new DtoPerson(model.Id,
            model.Name,
            model.Age,
            model.Address,
            model.Work);
    }
}