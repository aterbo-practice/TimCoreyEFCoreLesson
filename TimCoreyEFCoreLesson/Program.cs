using System;

namespace TimCoreyEFCoreLesson
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}

/************************************************************
NOTES: Tim thinks EF pushes Data Access higher into the UI Layer, when it should be separate
(probably in a class library or something). This example has Data Access folder because it's
in line with how EF works, but he prefers to put it all in a separate library

Code-First EF creation
Steps:
1. Create basic Models (Email, Phone, Contact)
2. Add NuGet packages
3. Create appsettings.json with trusted SQL Server connection string from 
    connectionstrings.com
4. Create Data Access layer by setting up Context
    a. ContactContext : DbContext
    b. Standard console app builder/config/appsettings setup    
    c. Override OnConfiguring to set UseSqlServer()
    





*************************************************************/