-- Wait for SQL Server to be ready
WAITFOR DELAY '00:00:05';

-- Create ProductDB if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ProductDB')
BEGIN
    CREATE DATABASE ProductDB;
    PRINT 'ProductDB created successfully';
END
ELSE
BEGIN
    PRINT 'ProductDB already exists';
END
GO

