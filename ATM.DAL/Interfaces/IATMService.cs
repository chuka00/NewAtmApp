using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATM.DAL.Interfaces
{
    public interface IATMService: IDisposable
    {
        Task Deposit();
        Task InteractiveTransfer();
        Task Transfer(int sender, int receiver, decimal amount);
        Task Withdraw();
    }
}
