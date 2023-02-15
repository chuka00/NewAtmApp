using System;
using System.Collections.Generic;
using System.Text;

namespace ATM.DAL.Model
{
    public class UserViewModel
    {
            public string Name { get; set; }
            public Guid UserId { get; set; }
            public int cardnumber { get; set; }
            public int cardPin { get; set; }
            public decimal balance { get; set; }
            public bool? status { get; set; }

    }
}
