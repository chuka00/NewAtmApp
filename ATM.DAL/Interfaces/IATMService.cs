using ATM.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATM.DAL.Interfaces
{
    public interface IATMService: IDisposable
    {
        Task<UserViewModel> CheckCardNumber(string cardNumber);
        Task Deposit();
        Task InteractiveTransfer();
        Task Transfer(int sender, int receiver, decimal amount);
        Task Withdraw();
       
        Task CheckBalance(Guid id);
    }
}
