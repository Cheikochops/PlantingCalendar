IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'plantbase')
BEGIN
EXEC('CREATE SCHEMA plantbase')
END