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
        public void createDB()
        {
            String ConnectionString;
            connection = new SqlConnection(@"Data Source=.;Initial Catalog=NewBzAtmApp;Integrated Security=True");

            ConnectionString = "CREATE DATABASE NewBzAtmApp";

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

        public void createUserTable()
        {

            connection = new SqlConnection(@"Data Source=.;Initial Catalog=NewBzAtmApp;Integrated Security=True");

            string createQ = "CREATE TABLE Users( id INT UNIQUE IDENTITY(1,1) NOT NULL," +
                    "userId uniqueidentifier NOT NULL  DEFAULT newid()," +
                    "name VARCHAR(70) NOT NULL, " +
                    "cardNumber VARCHAR(15) NOT NULL UNIQUE, " +
                    "cardPin VARCHAR(4) NOT NULL, " +
                    "balance DECIMAL(38,2) NOT NULL, " +
                    "status BIT NOT NULL, " +
                    "PRIMARY KEY(Id))";

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

        public void insertUserDemoData()
        {
            connection = new SqlConnection(@"Data Source=.;Initial Catalog=NewBzAtmApp;Integrated Security=True");

            string insertQuery =
               $"INSERT INTO USERS (Name, cardNumber, cardPin, balance,status)   " +
               $" VALUES ('John Doe','12345678910','1234',200000.89,1), " +
               $" ('Jane Doe', '10987654321','4321', 500000000.00,1), " +
               $" ('Zuri Micheal', '1122334455', '4545', 800000.17,1)";




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
