﻿using ATM.DAL;
using ATM.DAL.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ATM.Logic
{
    public class Authentication
    {
        private readonly ATMDBContext _dbContext;
        private readonly bool _disposed;

        public Authentication(ATMDBContext atmDBConnect)
        {
            _dbContext = atmDBConnect;
        }

        public async Task<UserViewModel> CheckCardNumber(string cardnumber)
        {
            UserViewModel user = new UserViewModel();
            try
            {
                SqlConnection sqlConn = await _dbContext.OpenConnection();

                Console.WriteLine("Please enter your card number:");
                string cardnumber = Console.ReadLine();

                string getUserInfo = $"SELECT Users.name,Users.userId,Users.cardPin FROM Users WHERE cardNumber = @cardnumber";
                await using SqlCommand command = new SqlCommand(getUserInfo, sqlConn);
                command.Parameters.AddRange(new SqlParameter[]
                {
            new SqlParameter
            {
                ParameterName = "@cardnumber",
                Value = cardnumber,
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Size = 15
            }
                });

                using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                {
                    while (dataReader.Read())
                    {
                        user.Name = dataReader["name"].ToString();
                        user.UserId = (Guid)dataReader["userId"];
                        user.cardPin = Convert.ToInt32(dataReader["cardPin"]);
                    }
                }

                return user;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
            return user;
        }

    }
}
