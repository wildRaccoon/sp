using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using sp.auth.app.infra.behaviours;
using sp.auth.app.infra.config;
using sp.auth.app.interfaces;
using sp.auth.persistence.services.hash;
using sp.auth.persistence.services.token;

namespace sp.auth.persistence
{
    public static class SpAuthServiceCollectionExtension
    {
        public static IServiceCollection AddSpAuthServices(this IServiceCollection entity, IConfiguration conf)
        {
            entity.TryAddSingleton<IHashService,HashService>();
            entity.TryAddSingleton<ITokenService,TokenService>();

            var cfg = new SpAuthConfig();
            conf.GetSection("sp.auth").Bind(cfg);
            
            entity.AddSingleton(cfg.hash);
            entity.AddSingleton(cfg.authenticate);

            entity.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            entity.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            
            entity.AddMediatR();
            
            return entity;
        }
    }
}