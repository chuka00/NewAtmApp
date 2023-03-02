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
        ATMService ATMService = new ATMService(new ATMDBContext());
        public async Task Run()
        {
            ATMDBService aTMDBService = new ATMDBService();
           // aTMDBService.BeginDbOperations();
             
            using (IATMService aTMServices = new ATMService(new ATMDBContext()))
            {
                // while (true)
                
                    try
                    {
                        Console.WriteLine("Welcome To CkBank");
                       return1: Console.WriteLine("\nPlease Insert Your Card Number:  \n");
                        string cardNumber = Console.ReadLine();
                        var user = await aTMServices.CheckCardNumber(cardNumber);
                        if (user.Name != null)
                        {

                            return2: Console.WriteLine($"\t \tHello {user.Name} \n \t Please Insert CardPin: ");
                            int pinNumber;
                            bool cardPin = int.TryParse(Console.ReadLine(), out pinNumber);

                            if ((user.cardPin == pinNumber) && cardPin)
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
                                        await aTMServices.InteractiveTransfer();
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
                //}
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

