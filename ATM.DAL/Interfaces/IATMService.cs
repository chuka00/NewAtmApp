using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATM.DAL.Interfaces
{
    public interface IATMService: IDisposable
    {
        Task Deposit(int id, decimal amount);
        Task transfer(int sender, int receiver, decimal amount);
        Task Withdraw(int id, decimal amount);
    }
}
