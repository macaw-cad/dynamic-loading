# Dynamic loading by reflection with donnet core

This project shows how you can dynamically load modules with dotnet core using reflection. In the folder you will find a solution that contains five projects. Each project has it's own responsabilty.
Projects:
- Macaw.DynamicLoading.WebApp (Web app)
- Macaw.DynamicLoading.Domain (class with interfaces)
- Macaw.DynamicLoading.ModuleRegistration (class to load modules)
- Macaw.DynamicLoading.List (class with implementation #list)
- Macaw.DynamicLoading.MemoryCache (class with implementation #MemoryCache)

# Web app
This is a basic dotnet core MVC Web app. This Web app relies on dependecy injection to dynamically load the the implemenations. The Web app only has a dependency on the Domain and ModuleRegistration projects, but not to the implemenation projects. The implemenation projects will be dynamically loaded into the memory by the ModuleRegistration. The `Startup.cs` file is responsible for the dependency injection of the `IoC container`. It relies on the extension method in the class `Macaw.DynamicLoading.ModuleRegistration` to load the required module.

# Domain
The domain only contains the interfaces and models that the Web app needs to do it's work. In the `Services` folder you'll find the interface `IPersonRepository`. This interface will be used for the implemenation in the projects (`Macaw.DynamicLoading.List` and `Macaw.DynamicLoading.MemoryCache`). Another important interface is the 'IRegisterModule'. This interface will be used to dynamically find these assemblies. In the next paragraph, it will be explained how this works.

# Dynamic module loading
For dynamically loading the modules, an extensions method was created on top of the `IServiceCollection`* interface. In the project `Macaw.DynamicLoading.ModuleRegistration` there is the file `RegisterModulesExtensions.cs`. This file contains an extension method that will scan and load the assemblies. This method will scan all the `Macaw.DynamicLoading.*.dll` assemlbies that are located in the output folder of the Web app project. Only projects that implements the `IRegisterModule` from the `Macaw.DynamicLoading.Domain` class will be loaded. 

\* the interface is located in the namespace `Microsoft.Extensions.DependencyInjection`

```
private static void AddModulesToServiceCollection(IServiceCollection services,
IEnumerable<Assembly> assembliesToScan)
{
    // Search for IRegisterModule interface
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

> You can't run both scripts at once, because it uses the same output folder to run the Web app. Shutdown the current command, before running the next script.

The two command scripts are mostly the same. It all starts with deleting all the `Macaw.DynamicLoading` assemlbies from the  Web app output folder. Then it will do a nuget restore, followed up by a build of the solution. 

After that, the script differs from each other. In the `RunWithList.cmd` script it will copy the assembly `Macaw.DynamicLoading.List.dll` to the output folder of the Web app. The `RunWithMemoryCache.cmd` script will copy the `Macaw.DynamicLoading.ModuleRegistration.dll` to the output folder.

After that it will call your browser to open a window to the url `http://localhost:5000`. Directly after that it will spin up the Web app project running on port `5000`. The website should be displayed in your browser. Check the assemblyname on the screen to see that the module was dynamically loaded into memory.

I hope you will enjoy this code. Remember to always have fun will coding!