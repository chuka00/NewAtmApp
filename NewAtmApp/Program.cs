using ATM.DAL;
using ATM.Logic;
namespace NewAtmApp
{
    public class Program
    {
        static  void Main (string[] args)
        {
            ATMOperations aTMOperations = new ATMOperations();
            ATMDBService aTMDBService = new ATMDBService();
            //aTMDBService.BeginDbOperations();
            aTMOperations.Run();
        }
    }
}