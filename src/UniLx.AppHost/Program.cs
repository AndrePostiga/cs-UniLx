var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var postgres = builder
    .AddPostgres("postgresdb")
    .WithImage("postgis/postgis")
    .WithEndpoint(port: 5432, targetPort: 5432, name:"postgres-endpoint", isExternal: true)
    .WithDataVolume()
    .WithBindMount(
        "./postgis/add-postgis-user.sql",
        "/docker-entrypoint-initdb.d/02-init-user.sql")
    .WithHealthCheck();

var pgAdmin = postgres
    .WithPgAdmin()
    .WaitFor(postgres);

var apiService = builder.AddProject<Projects.UniLx_ApiService>("apiservice")    
    .WithReference(postgres)
    .WaitFor(postgres);

builder.AddProject<Projects.UniLx_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

builder.Build().Run();
