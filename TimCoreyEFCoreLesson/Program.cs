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
    connectionstrings.com. Set to "Copy if newer" in properties.
4. Create Data Access layer by setting up Context
    a. ContactContext : DbContext
    b. Standard console app builder/config/appsettings setup    
    c. Override OnConfiguring to set UseSqlServer()
    d. Add DbSet for each model, corresponds to a table 
5. Use package manager to add migrations and create Db
    a. Console -> Add-Migration CreateDatabase
    b. Generates Migrations folder and code (CreateDatabase is the migration name, not a command)
    c. Up and Down scripts -> give you methods to move forward and backward through iterations
        of the Db structure, if you need to move forward or roll back changes in the future
    d. Note EF infers a lot of information - Id is primary key, constraints like PrimaryKey, 
        pluralizes table names, adds relationship Id columns between tables based on the Lists
        we created (FK named after the ID that it's referencing), some nullables are true but 
        you might want to change. Emails pull in as EmailAddresses per the Contact class.
        Foreight Key set up automatically between emails and contacts. Indexes created, too.
    e. There is also a code-behind file with more specifications like data type (VARCHAR(max))
    f. DO NOT IGNORE MIGRATIONS JUST BECAUSE THEY'RE CREATED. It's part of our code base that
        must be maintained. Need to understand it so that you can diagnose problems.
6. Push migration to production to analyze issues with the automatic setup
    a. PM Console -> Update-Database
    b. This runs the code in the ContactContext.cs to set up the SQL db and connection string
        and builds the Db tables per the most recent migration. 
    c. Notice lots of data that we'd consider required are marked as nullable and can be empty

Stopped ~ 40 min
    



















    





*************************************************************/