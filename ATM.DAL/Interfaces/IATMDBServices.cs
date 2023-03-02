using System;
using System.Collections.Generic;
using System.Text;

namespace ATM.DAL.Interfaces
{
    public interface IATMDBServices
    {
        public void CreateDB();

        public void CreateUserTable();

        public void InsertUserDemoData();
        public void CreateTransactionTable();
    }
}
