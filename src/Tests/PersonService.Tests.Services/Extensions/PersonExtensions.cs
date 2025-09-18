using FluentAssertions;
using PersonService.Core.Models;

namespace PersonService.Tests.Services.Extensions;

public static class PersonExtensions
{
    public static Person ShouldBe(this Person real)
    {
        return real;
    }

    public static Person Equal(this Person real, Person expected)
    {
        real.Id.Should().Be(expected.Id);
        real.Name.Should().Be(expected.Name);
        real.Age.Should().Be(expected.Age);
        real.Work.Should().Be(expected.Work);
        real.Address.Should().Be(expected.Address);

        return real;
    }

}