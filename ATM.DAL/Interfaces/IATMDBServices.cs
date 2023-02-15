using System;
using System.Collections.Generic;
using System.Text;

namespace ATM.DAL.Interfaces
{
    public interface IATMDBServices
    {
        void createDB();

        void createUserTable();

        void insertUserDemoData();
    }
}
