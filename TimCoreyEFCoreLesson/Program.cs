using System;
using System.Linq;
using TimCoreyEFCoreLesson.Models;
using Microsoft.EntityFrameworkCore;

namespace TimCoreyEFCoreLesson
{
    class Program
    {
        static void Main(string[] args)
        {

            //CreateAndy();    //This has already been run, Andy has been created in Db
            ReadAll();
            ReadAllWithInclude();
            ReadById(1);
            UpdateFirstName(1, "Andrew");
            RemovePhoneNumber(1, "555-555-1234");
            //ErrorRemoveUser(1);
            RemoveUser(1);
            Console.WriteLine("I know EF");
            Console.ReadLine();
        }

        private static void CreateAndy()
        {
            var c = new Contact
            {
                FirstName = "Andy",
                LastName = "Terbo"
            };
            c.EmailAddresses.Add(new Email { EmailAddress = "test@aterbo.com" });
            c.EmailAddresses.Add(new Email { EmailAddress = "Work@workemail.com" });
            c.PhoneNumbers.Add(new Phone { PhoneNumber = "555-555-1234" });
            c.PhoneNumbers.Add(new Phone { PhoneNumber = "123-456-7890" });

            using (var contactContext = new ContactContext())
            {
                contactContext.Contacts.Add(c);
                contactContext.SaveChanges();
            }
        }

        //This pulls in all records in the Contacts table, but doesn't reach into associated 
        //email and phone number tables
        private static void ReadAll()
        {
            using (var contactContext = new ContactContext())
            {
                var records = contactContext.Contacts.ToList();

                foreach (var c in records)
                {
                    Console.WriteLine($"{c.FirstName} {c.LastName}");
                }
            }
        }

        //This pulls in all records in the Contacts table, but ties in the emails and phone
        //numbers in the other tables and associates them with the object returned
        private static void ReadAllWithInclude()
        {
            using (var contactContext = new ContactContext())
            {
                var records = contactContext.Contacts
                    .Include(e => e.EmailAddresses)
                    .Include(p => p.PhoneNumbers)
                    .ToList();

                foreach (var c in records)
                {
                    Console.WriteLine($"{c.FirstName} {c.LastName}");
                }
            }
        }

        private static void ReadById(int id)
        {
            using (var contactContext = new ContactContext())
            {
                var user = contactContext.Contacts.Where(c => c.Id == id).First();

                Console.WriteLine($"{user.FirstName} {user.LastName}");
            }
        }

        private static void UpdateFirstName(int id, string firstName)
        {
            using (var contactContext = new ContactContext())
            {
                var user = contactContext.Contacts.Where(c => c.Id == id).First();

                user.FirstName = firstName;
                contactContext.SaveChanges();
            }
        }

        private static void RemovePhoneNumber(int id, string phoneNumber)
        {
            using (var contactContext = new ContactContext())
            {
                var user = contactContext.Contacts
                     .Include(p => p.PhoneNumbers)
                     .Where(c => c.Id == id).First();

                user.PhoneNumbers.RemoveAll(p => p.PhoneNumber == phoneNumber);

                contactContext.SaveChanges();
            }
        }

        //This delete statement won't work, since it would leave orphaned records
        //from the PhoneNumbers and EmailAddresses. Note that we're getting JUST
        //the data from the Contacts table, not the associated email and phone
        public static void ErrorRemoveUser(int id)
        {
            using (var contactContext = new ContactContext())
            {
                var user = contactContext.Contacts.Where(c => c.Id == id).First();
                contactContext.Contacts.Remove(user);
                contactContext.SaveChanges();
            }
        }

        //This delete statement will work, since we are cascading the Include statements
        //to get all of the associated records that are being impacted by the delete.
        //This is good Db design
        public static void RemoveUser(int id)
        {
            using (var contactContext = new ContactContext())
            {
                var user = contactContext.Contacts
                                         .Include(p => p.PhoneNumbers)
                                         .Include(e => e.EmailAddresses)
                                         .Where(c => c.Id == id).First();
                contactContext.SaveChanges();
            }
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
7. Create method to create dummy user data-- CreateAndy()
    a. Note he creates instance of Context within a using statement exactly when need.
8. Create ReadAll methods to pull down all records. ToList() triggers the actual retrieval and 
   manipulation of the data down to C#. Think about what data you really NEED to pull down
   prior to making your query.
    a. ReadAll() doesn't return the associated emails and phone numbers from other tables
    b. Include() does return the extra data from other tables. Similar to Join in SQL
9. These ReadAll methods are easy to impliment, and it will translate all this code to actual
   SQL queries that do all the translation and work to and from the Db and C#
    a. NOTE - EF is smart but can make inefficient SQL queries, especially if you aren't careful
        when structuring C# code.
    b. Tim recommends using SSMS -> Tools -> Monitoring to check what queries youe EF is 
        ACTUALLY running via, and do tuning to your queries as necessary 
            * Monitor what you need vs duplicate data that EF is pulling down
            * Consider splitting EF queries into two pieces
            * Watch if you're pulling IEnumerables (not creating objects) vs actually processing 
                data through a ToList or other methods.
    c. When using the .Include joins, you can get lots of duplicate data coming from the Db,
        So carefully think through what and why you're getting data and join-type queries.
10. ReadById(), search for Id with Where and First
    a. Uses SQL stored procedure that Tim says we should never use otherwise, 
    b. SQL query returns TOP 1, which is a good query. Only pulls the number of records requested
        from the server, so less network traffic. (Dapper can pull down whatever you ask for first,
        then only filters the number requested once it's on your machine)
11. RemovePhoneNumbers(), RemoveAll on PhoneNumbers
    a. When looking at the SQL, just breaks the link by removing the ContactId. It left the actual 
        phone number and entry, just broke the link to other tables. 
    b. Note that in the migration the CreateTable for the phone numbers says 
        onDelete: ReferentialAction.Restrict
12. RemoveUser - Note that trying to remove only the user and not the associated phone numbers and
    emails won't work. Need to use the include statements to get all records that will be affected
13. Improving the SQL and Db structure, such as maximum lengths, etc.
    a. When building the model files, add "decorators" above the parameter:
        [MaxLength(50)]
        [Required]  
        [Column(TypeName = "varchar(100")] 
    b. Since decorators were added, you need to make a new migration in the package manager
        Add-Migration ImprovedColumns
    c. Will warn you that data may be lost, since you've limited column length and you now have
        required columns
    d. Update-Database    This update will fail if you have a null in your Db where nulls are 
        now limited, such as the nulls that your RemovePhoneNumbers created
    e. When update fails, you need to delete the bad data in your Db manually


    



















    





*************************************************************/