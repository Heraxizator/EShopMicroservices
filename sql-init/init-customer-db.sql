-- Wait for SQL Server to be ready
WAITFOR DELAY '00:00:05';

-- Create CustomerDB if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CustomerDB')
BEGIN
    CREATE DATABASE CustomerDB;
    PRINT 'CustomerDB created successfully';
END
ELSE
BEGIN
    PRINT 'CustomerDB already exists';
END
GO

