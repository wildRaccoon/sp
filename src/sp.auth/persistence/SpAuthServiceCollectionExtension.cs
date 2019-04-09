using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using sp.auth.app.interfaces;
using sp.auth.persistence.services.hash;
using sp.auth.persistence.services.token;

namespace sp.auth.persistence
{
    public static class SpAuthServiceCollectionExtension
    {
        public static IServiceCollection AddSpAuthServices(this IServiceCollection entity)
        {
            entity.TryAddSingleton<IHashService,HashService>();
            entity.TryAddSingleton<ITokenService,TokenService>();
            
            return entity;
        }
    }
}