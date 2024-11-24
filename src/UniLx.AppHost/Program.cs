using Aspire.Hosting;
using TraceLens.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgresdb")    
    .WithLifetime(ContainerLifetime.Persistent)
    .WithImage("postgis/postgis")
    .WithEndpoint(port: 5432, targetPort: 5432, name: "postgres-endpoint", isExternal: true)
    .WithDataVolume()    
    .WithBindMount(
        "./postgis/add-postgis-user.sql",
        "/docker-entrypoint-initdb.d/02-init-user.sql")
    .WithArgs(
        "-c", "logging_collector=off",
        "-c", "log_statement=all",
        "-c", "log_min_duration_statement=0",
        "-c", "log_destination=stderr",
        "-c", "client_min_messages=log",
        "-c", "log_min_messages=log",
        "-c", "log_error_verbosity=default"
    );

var pgAdmin = postgres
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

var apiService = builder.AddProject<Projects.UniLx_ApiService>("apiservice")    
    .WithReference(postgres)
    .WaitFor(postgres);

builder.AddTraceLens();

builder.Build().Run();
