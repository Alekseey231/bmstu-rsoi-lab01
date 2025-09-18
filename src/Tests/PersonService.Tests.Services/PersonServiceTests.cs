using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PersonService.Core.Exceptions;
using PersonService.Core.Interfaces;
using PersonService.Tests.Services.Extensions;
using PersonService.Tests.Services.Helpers;

namespace PersonService.Tests.Services;

public class PersonServiceTests
{
    private readonly NullLogger<PersonService.Services.PersonService.PersonService> _logger;
    
    public PersonServiceTests()
    {
        _logger = NullLogger<PersonService.Services.PersonService.PersonService>.Instance;
    }

    
    [Fact]
    public async Task Creating_a_person()
    {
        var person = PersonMother.CreatePerson();
        var mock = new Mock<IPersonRepository>();
        mock.Setup(r => r.CreatePersonAsync(person.Name, person.Age, person.Address, person.Work))
            .ReturnsAsync(person);
        
        var sut = new PersonService.Services.PersonService.PersonService(mock.Object, _logger);
        
        var createdPerson = await sut.CreatePersonAsync(person.Name,
            person.Age,
            person.Address,
            person.Work);
        
        createdPerson.ShouldBe().Equal(person);
        mock.Verify(r => r.CreatePersonAsync(person.Name, person.Age, person.Address, person.Work), Times.Once);
    }

    [Fact]
    public async Task Getting_a_person_by_id()
    {
        var person = PersonMother.CreatePerson();
        var stub = new Mock<IPersonRepository>();
        stub.Setup(r => r.GetPersonByIdAsync(person.Id))
            .ReturnsAsync(person);
        
        var sut = new PersonService.Services.PersonService.PersonService(stub.Object, _logger);
        
        var gotPerson = await sut.GetPersonByIdAsync(person.Id);
        
        gotPerson.ShouldBe().Equal(person);
    }

    [Fact]
    public async Task Getting_not_exits_person_by_id()
    {
        var person = PersonMother.CreatePerson();
        var stub = new Mock<IPersonRepository>();
        stub.Setup(r => r.GetPersonByIdAsync(person.Id))
            .ThrowsAsync(new PersonNotFoundException());
        var sut = new PersonService.Services.PersonService.PersonService(stub.Object, _logger);
        
        var act = async () =>
        {
            await sut.GetPersonByIdAsync(person.Id);
        };

        await act.Should().ThrowAsync<PersonNotFoundException>();
    }
}