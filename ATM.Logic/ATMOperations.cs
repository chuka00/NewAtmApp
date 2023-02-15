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
        start: Console.WriteLine("\n Please Insert Your Card Number:  \n");
            string cardnumber = Console.ReadLine();

            using (IATMService aTMServices = new ATMService(new ATMDBContext()))
            {
                while (true)
                {
                    try
                    {
                        //Some code
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

