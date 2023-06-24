using DatabaseContext.GenericRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseContext;

public static class DatabaseContextModules
{
    public static void ApplyDataBaseDI(this IServiceCollection services, string dbConnection)
    {
        services.AddDbContext<TaskManagmentDBContext>(options => options.UseSqlServer(dbConnection), ServiceLifetime.Transient);

        services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
    }
}
