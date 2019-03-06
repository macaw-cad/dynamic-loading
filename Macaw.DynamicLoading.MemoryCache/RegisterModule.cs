using Macaw.DynamicLoading.Domain;
using Macaw.DynamicLoading.Domain.Services;
using Macaw.DynamicLoading.MemoryCache.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Macaw.DynamicLoading.MemoryCache
{
    public class RegisterModule : IRegisterModule
    {
        public void RegisterComponents(IServiceCollection services)
        {
            services.AddSingleton<IPersonRepository, PersonRepository>();
        }
    }
}