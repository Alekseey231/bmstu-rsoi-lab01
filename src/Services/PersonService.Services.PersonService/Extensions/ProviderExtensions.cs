using Microsoft.Extensions.DependencyInjection;
using PersonService.Core.Interfaces;

namespace PersonService.Services.PersonService.Extensions;

public static class ProviderExtensions
{
    public static void AddPersonService(this IServiceCollection services)
    {
        services.AddScoped<IPersonService, PersonService>();
    }
}
