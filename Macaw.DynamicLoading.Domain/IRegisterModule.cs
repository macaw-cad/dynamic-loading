using Microsoft.Extensions.DependencyInjection;

namespace Macaw.DynamicLoading.Domain
{
    public interface IRegisterModule
    {
        void RegisterComponents(IServiceCollection services);
    }
}
