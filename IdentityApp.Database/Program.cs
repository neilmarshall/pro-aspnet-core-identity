using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using DbUp;

namespace IdentityApp.Database
{
    class Program
    {
        private class ConnectionStrings
        {
            public string Default { get; set; }
        }

        static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<ConnectionStrings>()
                .Build();

            var connectionString = configuration.GetConnectionString("Default");

            EnsureDatabase.For.PostgresqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .PostgresqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}
