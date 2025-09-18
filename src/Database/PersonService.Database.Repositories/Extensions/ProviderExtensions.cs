using Microsoft.Extensions.DependencyInjection;
using PersonService.Core.Interfaces;

namespace PersonService.Database.Repositories.Extensions;

public static class ProviderExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPersonRepository, PersonRepository>();
    }
}