using ATM.Logic;
namespace NewAtmApp
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            // Console.WriteLine("Hello, World!");
            //ATMOperations aTMOperations = new ATMOperations();
            await ATMOperations.Run();
        }
    }
}