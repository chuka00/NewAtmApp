using ATM.DAL.Interfaces;
using ATM.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Logic
{
    public class ATMOperations
    {

        public static async Task Run()
        {

            Console.WriteLine("Welcome To Bz ATM");
          return1:  Console.WriteLine("\n Please Insert Your Card Number:  \n");
            //string cardnumber = Console.ReadLine();

            using (IATMService aTMServices = new ATMService(new ATMDBContext()))
            {
                 while (true)
                {
                    try
                    {
                        var user = await aTMServices.CheckCardNumber();
                        if (user.Name != null)
                        {

                        return2: Console.WriteLine($"\t \tHello {user.Name} \n \t Please Insert Card Pin: ");
                            int pinNumber;
                            bool cardpin = int.TryParse(Console.ReadLine(), out pinNumber);

                            if ((user.cardPin == pinNumber) && cardpin)
                            {
                                Console.Clear();
                            return3: PrintOperations();
                                string option = Console.ReadLine();
                                //decimal amount;
                                switch (option)
                                {
                                    case "1":
                                        await aTMServices.Deposit();
                                        break;
                                    case "2":
                                        await aTMServices.Withdraw();
                                        break;
                                    case "3":
                                        await aTMServices.CheckBalance(user.UserId);
                                        break;
                                    case "4":

                                        Console.WriteLine("Enter Your Account ID:");
                                        int senderId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter Receiver's Account ID:");
                                        int receiverId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter Amount to Transfer:");
                                        decimal amount = decimal.Parse(Console.ReadLine());

                                        await aTMServices.Transfer(senderId, receiverId, amount);

                                        break;
                                    case "5":
                                        Console.WriteLine("Have a great day, goodbye!");
                                        Environment.Exit(0);
                                        break;
                                    default:
                                        goto return3;

                                }
                            }
                            else
                            {
                                Console.WriteLine("Incorrect card Pin");
                                goto return2;
                            }

                        }
                        else
                        {
                            Console.WriteLine("incorrect card number");
                            goto return1;
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
        }

        public static void PrintOperations()
        {


            Console.WriteLine("Kindly select an operation to perform\n" +
                "1. Deposit \n" +
                "2. Withdraw \n" +
                "3. Show Balance \n" +
                "4. Transfer \n" +
                "5. Exit");

        }
    }
}

