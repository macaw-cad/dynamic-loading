using Macaw.DynamicLoading.Domain;
using Macaw.DynamicLoading.Domain.Services;
using Macaw.DynamicLoading.List.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Macaw.DynamicLoading.List
{
    public class RegisterModule : IRegisterModule
    {
        public void RegisterComponents(IServiceCollection services)
        {
            services.AddSingleton<IPersonRepository, PersonRepository>();
        }
    }
}