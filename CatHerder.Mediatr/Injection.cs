
using CatHerder.Mediatr.Pipelines;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CatHerder.Mediatr;

public class Injection
{
    public static void Register(IServiceCollection services)
    {
        var applicationAssembly = typeof(Injection).Assembly;
        services.AddMediatR(typeof(Injection));

        var validators = applicationAssembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(i => typeof(IValidationHandler<,>).Name == i.Name));

        foreach (var validator in validators)
        {
            var @interface = validator.GetInterfaces().FirstOrDefault(i => typeof(IValidationHandler<,>).Name == i.Name);
            services.AddSingleton(@interface, validator);
        }

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggerPipeline<,>));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggerPipeline<,>));

        //services.AddValidatorsFromAssembly(typeof(Circus.Mediatr.Users.Add).Assembly);
    }
}
