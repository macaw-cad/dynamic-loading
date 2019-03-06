using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Macaw.DynamicLoading.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Macaw.DynamicLoading.ModuleRegistration.Extensions
{
    public static class RegisterModulesExtensions
    {
        public static IServiceCollection RegisterModules(this IServiceCollection services)
        {
            AddModulesToServiceCollection(services, GetAallCustomAssemblies());
            return services;
        }

        private static IEnumerable<Assembly> GetAallCustomAssemblies()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Directory.GetFiles(path, "Macaw.DynamicLoading.*.dll").Select(Assembly.LoadFile);
        }

        private static void AddModulesToServiceCollection(IServiceCollection services, IEnumerable<Assembly> assembliesToScan)
        {
            var interfaceType = typeof(IRegisterModule);
            var types = assembliesToScan
                .SelectMany(s => s.GetTypes())
                .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass)
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var module = instance as IRegisterModule;
                module.RegisterComponents(services);
            }
        }
    }
}