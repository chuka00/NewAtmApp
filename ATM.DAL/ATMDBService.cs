using ATM.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace ATM.DAL
{
    public class ATMDBService : IATMDBServices
    {
        private SqlConnection connection;
        private bool _disposed;

        public void CreateDB()
        {
            String ConnectionString;
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            ConnectionString = @" IF NOT EXISTS(SELECT name FROM sys.databases WHERE name = 'CkBank')
                             BEGIN
                             CREATE DATABASE CkBank;
                             END";

            SqlCommand myCommand = new SqlCommand(ConnectionString, connection);
            try
            {
                connection.Open();
                myCommand.ExecuteNonQuery();
                Console.WriteLine("DataBase is Created Successfully");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }


        public void CreateUserTable()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CkBank;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            string createQ = @"IF NOT EXISTS(SELECT * FROM sys.tables WHERE name='[User]')
                BEGIN
                    CREATE TABLE [User] (
                        UserId INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(50) NOT NULL,
                        CardNumber NVARCHAR(16) NOT NULL,
                        CardPin NVARCHAR(4) NOT NULL,
                        AccountBalance DECIMAL(18, 2) NOT NULL
                    )
                END";

            SqlCommand myCommand = new SqlCommand(createQ, connection);
            try
            {
                connection.Open();
                myCommand.ExecuteNonQuery();
                Console.WriteLine("User Table Created Successfully");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }
        //CreateUserTable();

        public void InsertUserDemoData()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CkBank;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            string insertQuery =
               $"INSERT INTO [User] (Name, CardNumber, CardPin, AccountBalance)   " +
               $" VALUES ('Dav Hart','33745649437456','1234',100000.89), " +
               $" ('Rayn Jim', '33302833330287','5555', 500000000.00), " +
               $" ('Pam Micheal', '49684709528753', '1000', 800000.17)";




            SqlCommand command = new SqlCommand(insertQuery, connection);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("User Data Created Successfully");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }
        //insertUserDemoData();
        public void CreateTransactionTable()
        {
            SqlConnection connection = new SqlConnection(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog=CkBank; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");

            string createQ = @"IF NOT EXISTS(SELECT * FROM sys.tables WHERE name='[Transaction]')
                BEGIN
                    CREATE TABLE [Transaction] (
                        TransactionId INT PRIMARY KEY IDENTITY(1,1),
                        UserId INT NOT NULL,
                        Amount DECIMAL(18, 2) NOT NULL,
                        Description NVARCHAR(100) NOT NULL,
                        TransactionType NVARCHAR(10) NOT NULL,
                        TransactionDate DATETIME NOT NULL,
                        CONSTRAINT FK_Transaction_User FOREIGN KEY (UserId) REFERENCES [User](UserId)
                    )END";

            SqlCommand myCommand = new SqlCommand(createQ, connection);
            try
            {
                connection.Open();
                myCommand.ExecuteNonQuery();
                Console.WriteLine("Transaction Table Created Successfully");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }

        public void BeginDbOperations()
        {
            CreateDB();

            CreateUserTable();

            InsertUserDemoData();
            CreateTransactionTable();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                connection.Dispose();
            }

            _disposed = true;
        }
        public void Dispose()
        {

            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
