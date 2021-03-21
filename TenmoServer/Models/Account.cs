using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Account
    {
        public int Account_Id { get; set; }
        public int User_Id { get; set; }
        public Decimal Balance { get; set; }

    }
}
