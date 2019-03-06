del .\Macaw.DynamicLoading.WebApp\bin\Debug\netcoreapp2.2\Macaw.DynamicLoading.*.dll /F /Q
dotnet restore
dotnet build Macaw.DynamicLoading.sln
copy .\Macaw.DynamicLoading.MemoryCache\bin\Debug\netcoreapp2.2\Macaw.DynamicLoading.MemoryCache.dll .\Macaw.DynamicLoading.WebApp\bin\Debug\netcoreapp2.2\
start http://localhost:5000
dotnet run --project .\Macaw.DynamicLoading.WebApp\Macaw.DynamicLoading.WebApp.csproj