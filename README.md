# Dynamic loading by reflection with donnet core

This project shows how you can dynamically load modules with dotnet core using reflection. In the folder you will find a solutuion that contains five projects:
- Macaw.DynamicLoading.WebApp (Web app)
- Macaw.DynamicLoading.Domain (class with interfaces)
- Macaw.DynamicLoading.ModuleRegistration (class to load modules)
- Macaw.DynamicLoading.List (class with implementation #list)
- Macaw.DynamicLoading.MemoryCache (class with implementation #MemoryCache)

# Web app
This is a basic dotnet core MVC Web app. The api relies on the `IPersonRepository` interface from the Domain class. It interacts with the interface in the PersonController. The `Startup.cs` file responsible for registering the modules in the `IoC contaier`. The call is done on line #31.

# Domain
The domain only contains the interfaces and models that the Web app needs to do it's work. In the `Services` folder you'll find the interface `IPersonRepository`. This interface will be used by the implemenation in the projects (`Macaw.DynamicLoading.List` and `Macaw.DynamicLoading.MemoryCache`). Another important interface is the 'IRegisterModule' interface. This interface will be used to dynamically search the assemblies for this interface. In the next paragraph will be explained how this works.

# Dynamic module loading
For dynamically loading the modules, an extensions method was created on top of the `IServiceCollection`* interface. In the project `Macaw.DynamicLoading.ModuleRegistration` -> `RegisterModulesExtensions.cs` file, the extension method is located. This method will scan all the `Macaw.DynamicLoading.*.dll` assemlbies that are located in the output folder of the Web app project. Only projects that implemted the `IRegisterModule` from the `Macaw.DynamicLoading.Domain` class will be loaded. 

\* the interface is located in the namespace `Microsoft.Extensions.DependencyInjection`

```
private static void AddModulesToServiceCollection(IServiceCollection services,
IEnumerable<Assembly> assembliesToScan)
{
    // Search for this interface
    var interfaceType = typeof(IRegisterModule);
    
    // Search for the assemblies that implements the interfaceType and is a class
    var types = assembliesToScan
        .SelectMany(s => s.GetTypes())
        .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass)
        .ToList();

    // For every class that has been found, create an instance an register
    // that instance in the IoC container
    foreach (var type in types)
    {
        var instance = Activator.CreateInstance(type);
        var module = instance as IRegisterModule;
        module.RegisterComponents(services);
    }
}
```

> Make sure you only put in one assembly that implements the same interface within your module, otherwise the lastest registration for that specific interface will be used.

# Implementations of IPersonRepository interface
The two projects `Macaw.DynamicLoading.List` and `Macaw.DynamicLoading.MemoryCache` both have an implementation of the `IPersonRepository` interface. To run the Web app with one of them, you can run one of the two command scripts that are located in the root folder.
- RunWithList.cmd
- RunWithMemoryCache.cmd

The two command scripts  are mostly the same. It all starts with deleting all the `Macaw.DynamicLoading` assemlbies from the  Web app output folder. Then it will do a nuget restore, followed up by a build of the solution. 

After that the script differs. In the `RunWithList.cmd` script it will copy the assembly `Macaw.DynamicLoading.List.dll` to the output folder of the Web app. The `RunWithMemoryCache.cmd` script will copy the `Macaw.DynamicLoading.ModuleRegistration.dll` to the output folder.

After that it will call your browser to browse the url `http://localhost:5000`. Directly after that it will spin up the Web app project running on port `5000`.