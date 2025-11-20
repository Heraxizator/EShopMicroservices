-- Wait for SQL Server to be ready
WAITFOR DELAY '00:00:05';

-- Create OrderDB if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'OrderDB')
BEGIN
    CREATE DATABASE OrderDB;
    PRINT 'OrderDB created successfully';
END
ELSE
BEGIN
    PRINT 'OrderDB already exists';
END
GO

