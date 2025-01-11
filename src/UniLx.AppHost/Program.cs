var builder = DistributedApplication.CreateBuilder(args);

var kafka = builder.AddKafka("kafka")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEndpoint(name: "kafka-external", targetPort: 9092, port: 9092)
    .WithEndpoint(name: "kafka-internal", targetPort: 9093, port: 9093)
    .WithEndpoint(name: "kafka-controller", targetPort: 9094, port: 9094) // Controller listener for internal use only
    .WithEnvironment("KAFKA_ADVERTISED_LISTENERS", "PLAINTEXT://localhost:9092,PLAINTEXT_INTERNAL://kafka:9093")
    .WithEnvironment("KAFKA_LISTENERS", "PLAINTEXT://0.0.0.0:9092,PLAINTEXT_INTERNAL://0.0.0.0:9093,CONTROLLER://0.0.0.0:9094")
    .WithEnvironment("KAFKA_LISTENER_SECURITY_PROTOCOL_MAP", "PLAINTEXT:PLAINTEXT,PLAINTEXT_INTERNAL:PLAINTEXT,CONTROLLER:PLAINTEXT")
    .WithEnvironment("KAFKA_INTER_BROKER_LISTENER_NAME", "PLAINTEXT_INTERNAL")
    .WithEnvironment("KAFKA_CONTROLLER_LISTENER_NAMES", "CONTROLLER")
    .WithEnvironment("KAFKA_PROCESS_ROLES", "broker,controller")
    .WithEnvironment("KAFKA_NODE_ID", "1")
    .WithEnvironment("KAFKA_CONTROLLER_QUORUM_VOTERS", "1@localhost:9094")
    .WithEnvironment("KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR", "1")
    .WithEnvironment("KAFKA_TRANSACTION_STATE_LOG_MIN_ISR", "1")
    .WithEnvironment("KAFKA_TRANSACTION_STATE_LOG_REPLICATION_FACTOR", "1")
    .WithDataVolume(isReadOnly: false)
    .WithKafkaUI();



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

postgres
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<Projects.UniLx_ApiService>("apiservice")
    .WithReference(postgres)
    .WithReference(kafka)
    .WaitFor(postgres)
    .WaitFor(kafka);

await builder.Build().RunAsync();
