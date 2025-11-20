using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace Order.Microservice;

public class Program
{
    public static void Main(string[] args)
    {
        EnsureDatabaseCreated();
        CreateHostBuilder(args).Build().Run();
    }

    private static void EnsureDatabaseCreated()
    {
        try
        {
            var connectionString = "Server=order-db;Database=master;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;";
            var dbName = "OrderDB";
            
            for (int i = 0; i < 30; i++)
            {
                try
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{dbName}') CREATE DATABASE [{dbName}]";
                        command.ExecuteNonQuery();
                        Console.WriteLine($"{dbName} database created or already exists.");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Waiting for SQL Server... Attempt {i + 1}/30: {ex.Message}");
                    System.Threading.Thread.Sleep(2000);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error ensuring database created: {ex.Message}");
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
