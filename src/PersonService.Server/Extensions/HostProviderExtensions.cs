using Microsoft.EntityFrameworkCore;
using PersonService.Database.Context;

namespace PersonService.Server.Extensions;

public static class HostProviderExtensions
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var serviceScope = host.Services.CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<PersonServiceContext>()!;
        
        context.Database.Migrate();
        
        return host;
    }

}