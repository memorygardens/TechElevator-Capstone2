using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDAO
    {
        Account GetAccount(int userId);
        List<Transfer> GetTransfers(int transferId);
        void TransferToRegUser(int userId, int transferId, decimal amount);

        User UserIdFromAccountID(int accountId);
    }
}