using ATM.DAL;
using ATM.DAL.Interfaces;
using ATM.DAL.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ATM.Logic
{
    public class ATMService : IATMService
    {

        private readonly ATMDBContext _dbContext;
        private bool _disposed;

        public ATMService(ATMDBContext atmDBConnect)
        {
            _dbContext = atmDBConnect;
        }

        public async Task<UserViewModel> CheckCardNumber(string cardNumber)
        {
            UserViewModel user = new UserViewModel();
            try
            {
                SqlConnection sqlConn = await _dbContext.OpenConnection();

                string getUserInfo = $"SELECT Users.Name,Users.UserId,Users.CardPin FROM [User] WHERE CardNumber = @CardNumber";
                await using SqlCommand command = new SqlCommand(getUserInfo, sqlConn);
                command.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter
                {
                    ParameterName = "@CardNumber",
                    Value = cardNumber,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Size = 15
                }
                });


                using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                {
                    while (dataReader.Read())
                    {
                        user.Name = dataReader["Name"].ToString();
                        user.UserId = (Guid)dataReader["UserId"];
                        user.cardPin = Convert.ToInt32(dataReader["CardPin"]);
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




        public async Task Deposit()
        {
            try
            {
                // Prompt the user for the ID and amount to deposit
                Console.Write("Enter user ID: ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("Enter deposit amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                SqlConnection sqlConn = await _dbContext.OpenConnection();

                string getUserInfo = $"SELECT User.AccountBalance,User.UserId FROM [User] WHERE Id = @UserId";
                await using SqlCommand command = new SqlCommand(getUserInfo, sqlConn);
                command.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter
                {
                    ParameterName = "@UserId",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Size = 50
                }
                });
                UserViewModel user = new UserViewModel();
                using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                {
                    while (dataReader.Read())
                    {
                        user.balance = (decimal)dataReader["AccountBalance"];
                        user.UserId = (Guid)dataReader["UserId"];
                    }
                }
                user.balance = user.balance + amount;

                command.CommandText = $"UPDATE  [User] SET AccountBalance = {user.balance}  WHERE Id = @UserId";

                var result = await command.ExecuteNonQueryAsync();

                if (result > 0)
                {
                    DateTime myDateTime = DateTime.Now;
                    string sqlFormat = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string desc = $"Deposited a sum of {amount} into your account, you current balance is {user.balance}";
                    command.CommandText = $"INSERT INTO Transactions (UserId,receiverId,transactionType,desctiption,amount,createdAt)" +
                         $" VALUES (@sendId,null,'Deposit',@desc,@amount,1,@date)";
                    command.Parameters.AddRange(new SqlParameter[]
               {
                new SqlParameter
                {
                    ParameterName = "@sendId",
                    Value = user.UserId,
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input
                },
                 new SqlParameter
                {
                    ParameterName = "@desc",
                    Value = desc,
                    SqlDbType = SqlDbType.NText,
                    Direction = ParameterDirection.Input,

                },
                  new SqlParameter
                {
                    ParameterName = "@amount",
                    Value = amount,
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Input,

                },
                   new SqlParameter
                {
                    ParameterName = "@date",
                    Value = sqlFormat,
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,

                }

               });
                    await command.ExecuteNonQueryAsync();

                    Console.WriteLine("Deposit Was successful");
                }
                else
                {
                    Console.WriteLine("Deposit was unsuccessful");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
        }


        public async Task InteractiveTransfer()
        {
            try
            {
                Console.Write("Enter sender ID: ");
                int senderId = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter receiver ID: ");
                int receiverId = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter amount: ");
                decimal amount = Convert.ToDecimal(Console.ReadLine());

                await Transfer(senderId, receiverId, amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }



        public async Task Transfer(int sender, int receiver, decimal amount)
        {
            try
            {
                //open connection
                SqlConnection sqlConn = await _dbContext.OpenConnection();

                //senders info
                string senderInfo = $"SELECT Users.balance,Users.userId FROM Users WHERE Id = @senderId";
                SqlCommand command = new SqlCommand(senderInfo, sqlConn);
                command.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter
                {
                    ParameterName = "@senderId",
                    Value = sender,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Size = 50
                }
                });

                UserViewModel senderObj = new UserViewModel();
                using (SqlDataReader dataReaderSender = command.ExecuteReader())
                {
                    while (dataReaderSender.Read())
                    {
                        senderObj.balance = (decimal)dataReaderSender["balance"];
                        senderObj.UserId = (Guid)dataReaderSender["userId"];
                    }
                }

                if (amount > senderObj.balance)
                {
                    Console.WriteLine("Insucficient Balance");
                    Environment.Exit(0);
                }

                //Receivers Info

                string receiverInfo = $"SELECT Users.balance,Users.userId,Users.name FROM Users WHERE Id = @receiverId";
                SqlCommand command2 = new SqlCommand(receiverInfo, sqlConn);
                command2.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter
                {
                    ParameterName = "@receiverId",
                    Value = receiver,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Size = 50
                }
                });

                UserViewModel receiverObj = new UserViewModel();
                using (SqlDataReader dataReaderReceiver = command2.ExecuteReader())
                {
                    while (dataReaderReceiver.Read())
                    {
                        receiverObj.balance = (decimal)dataReaderReceiver["balance"];
                        receiverObj.UserId = (Guid)dataReaderReceiver["userId"];
                        receiverObj.Name = (string)dataReaderReceiver["name"];
                    }
                }

                // do transfer

                senderObj.balance -= amount;
                receiverObj.balance += amount;

                //update sender
                command.CommandText = $"UPDATE  Users SET balance = {senderObj.balance}  WHERE Id = @senderId";

                var result = command.ExecuteNonQuery();

                if (result > 0)
                {
                    //update receiver
                    command2.CommandText = $"UPDATE  Users SET balance = {receiverObj.balance}  WHERE Id = @receiverId";

                    var result2 = command2.ExecuteNonQuery();

                    if (result2 > 0)
                    {
                        DateTime myDateTime = DateTime.Now;
                        string sqlFormat = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        string desc = $"Transfered a sum of {amount} from your account to {receiverObj.Name}, your current balance is {senderObj.balance}";
                        command.CommandText = $"INSERT INTO Transactions (userId,receiverId,transactionType,desctiption,amount,status,createdAt)" +
                             $" VALUES (@sendId,@receiverId,'Transfer',@desc,@amount,1,@date)";
                        command.Parameters.AddRange(new SqlParameter[]
                   {
                new SqlParameter
                {
                    ParameterName = "@sendId",
                    Value = senderObj.UserId,
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input
                },
                 new SqlParameter
                {
                    ParameterName = "@receiverId",
                    Value = receiverObj.UserId,
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input
                },
                 new SqlParameter
                {
                    ParameterName = "@desc",
                    Value = desc,
                    SqlDbType = SqlDbType.NText,
                    Direction = ParameterDirection.Input,

                },
                  new SqlParameter
                {
                    ParameterName = "@amount",
                    Value = amount,
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Input,

                },
                   new SqlParameter
                {
                    ParameterName = "@date",
                    Value = sqlFormat,
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,

                }

                   });
                        command.ExecuteNonQuery();
                        Console.WriteLine($"Transfer Successful");
                    }
                    else
                    {
                        Console.WriteLine("system Error: Unable to complete transfer");
                    }
                }
                else
                {
                    Console.WriteLine("Unsuccessful Withdrawal");
                    Environment.Exit(0);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

        }



        public async Task Withdraw()
        {
            try
            {
                Console.WriteLine("Please enter user ID:");
                int id = int.Parse(Console.ReadLine());

                Console.WriteLine("Please enter amount to withdraw:");
                decimal amount = decimal.Parse(Console.ReadLine());

                SqlConnection sqlConn = await _dbContext.OpenConnection();

                string getUserInfo = $"SELECT Users.balance,Users.userId FROM Users WHERE Id = @UserId";
                SqlCommand command = new SqlCommand(getUserInfo, sqlConn);
                command.Parameters.AddRange(new SqlParameter[]
                {
            new SqlParameter
            {
                ParameterName = "@UserId",
                Value = id,
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Size = 50
            }
                });

                UserViewModel user = new UserViewModel();
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        user.balance = (decimal)dataReader["balance"];
                        user.UserId = (Guid)dataReader["userId"];
                    }
                }

                if (amount > user.balance)
                {
                    Console.WriteLine("Insufficient Balance");
                    Environment.Exit(0);
                }

                user.balance = user.balance - amount;

                command.CommandText = $"UPDATE Users SET balance = {user.balance} WHERE Id = @UserId";

                var result = command.ExecuteNonQuery();

                if (result > 0)
                {

                    DateTime myDateTime = DateTime.Now;
                    string sqlFormat = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string desc = $"Withdrew a sum of {amount} from your account, your current balance is {user.balance}";
                    command.CommandText = $"INSERT INTO Transactions (userId,receiverId,transactionType,desctiption,amount,status,createdAt)" +
                         $" VALUES (@sendId,null,'Withdraw',@desc,@amount,1,@date)";
                    command.Parameters.AddRange(new SqlParameter[]
                    {
                new SqlParameter
                {
                    ParameterName = "@sendId",
                    Value = user.UserId,
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input
                },
                 new SqlParameter
                {
                    ParameterName = "@desc",
                    Value = desc,
                    SqlDbType = SqlDbType.NText,
                    Direction = ParameterDirection.Input,

                },
                  new SqlParameter
                {
                    ParameterName = "@amount",
                    Value = amount,
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Input,

                },
                   new SqlParameter
                {
                    ParameterName = "@date",
                    Value = sqlFormat,
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,

                }

                    });
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Withdrawal Successful");
                }
                else
                {
                    Console.WriteLine("Unsuccessful Withdrawal");
                    Environment.Exit(0);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }


        public async Task CheckBalance(Guid id)
        {
            try
            {
                SqlConnection sqlConn = await _dbContext.OpenConnection();

                string getUserInfo = $"SELECT Users.balance,Users.userId FROM Users WHERE userId = @UserId";
                await using SqlCommand command = new SqlCommand(getUserInfo, sqlConn);
                command.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter
                {
                    ParameterName = "@UserId",
                    Value = id,
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    Size = 50
                }
                });
                UserViewModel user = new UserViewModel();
                using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                {
                    while (dataReader.Read())
                    {
                        user.balance = (decimal)dataReader["balance"];
                        user.UserId = (Guid)dataReader["userId"];
                    }
                }

                Console.WriteLine($"Your Balance is ${user.balance}");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

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
                _dbContext.Dispose();
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
