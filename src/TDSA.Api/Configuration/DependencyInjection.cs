using Microsoft.Extensions.DependencyInjection;
using TDSA.Business.Interfaces;
using TDSA.Business.Notificacoes;
using TDSA.Business.Services;
using TDSA.Data.Context;
using TDSA.Data.Repository;

namespace TDSA.Api.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ResolverDependencias(this IServiceCollection services)
        {
            services.AddScoped<TDSAContext>();
            services.AddScoped<INotificador, Notificador>();

            services.AddScoped<IMedicoService, MedicoService>();

            services.AddScoped<IMedicoRepository, MedicoRepository>();

            return services;
        }
    }
}
