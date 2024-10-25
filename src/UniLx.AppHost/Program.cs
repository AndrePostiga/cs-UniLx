var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var postgres = builder    
    .AddPostgres("postgresdb")
    .WithDataVolume()
    .WithHealthCheck();

var pgAdmin = postgres.WithPgAdmin().WaitFor(postgres);

var apiService = builder.AddProject<Projects.UniLx_ApiService>("apiservice")    
    .WithReference(postgres)
    .WaitFor(postgres);

builder.AddProject<Projects.UniLx_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

builder.Build().Run();
