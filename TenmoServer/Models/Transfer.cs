using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int TransferTypeId { get; set; }
        public int TransferStatusId { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }
    }

    public class TransferTypes
    {
        public int Transfer_Type_Id { get; set; }
        public int Transfer_Type_Desc { get; set; }
    }

    public class TransferStatus
    {
        public int Transfer_Status_Id { get; set; }
        public int Transfer_Status_Desc { get; set; }
    }
}
