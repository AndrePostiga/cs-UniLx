CREATE EXTENSION postgis;

CREATE ROLE myuser WITH LOGIN PASSWORD 'mypassword';

-- Grant permissions to create databases and roles
ALTER ROLE myuser CREATEDB;
ALTER ROLE myuser CREATEROLE;

-- Grant all privileges on the public schema
GRANT ALL PRIVILEGES ON DATABASE postgres TO myuser;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO myuser;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO myuser;
GRANT ALL PRIVILEGES ON ALL FUNCTIONS IN SCHEMA public TO myuser;

-- Set default privileges for new tables, sequences, and functions
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL PRIVILEGES ON TABLES TO myuser;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL PRIVILEGES ON SEQUENCES TO myuser;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL PRIVILEGES ON FUNCTIONS TO myuser;

