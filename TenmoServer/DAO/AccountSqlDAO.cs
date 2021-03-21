using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountDAO
    {
        private readonly string connectionString;

        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Account GetAccount(int userId)
        {
            Account account = new Account();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"Select * from accounts where user_id = @userid", conn);
                cmd.Parameters.AddWithValue("@userid", userId);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    account = RowToAccount(rdr);
                }
            }
            return account;
        }


        public void TransferToRegUser(int userId, int accountToId, decimal amount)
        {
            Account account = GetAccount(userId);

            if (account.Balance >= amount)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand(@"
                            BEGIN TRANSACTION
                            INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) 
                                VALUES ((select transfer_type_id from transfer_types where transfer_type_desc = 'Send'), 
                                (Select transfer_status_id from transfer_statuses where transfer_status_desc = 'Approved'), 
                                @userId, @accountToId, @amount)
                            UPDATE accounts set balance = balance - @amount where account_id = @userId
                            UPDATE accounts set balance = balance + @amount where account_id = @accountToId
                            COMMIT TRANSACTION
                            ", conn);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@accountToId", accountToId);
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw;
                }
            }
            else
            {
                Console.WriteLine("YA BROKE!");
            }
        }

        public List<Transfer> GetTransfers(int transferId)
        {
            List<Transfer> transfers = new List<Transfer>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT * FROM transfers" /*WHERE transfer_Id = @transferId*/, conn);
                    cmd.Parameters.AddWithValue("@transferId", transferId);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Transfer transfer = RowToTransfer(rdr);
                        transfers.Add(transfer);

                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return transfers;
        }

        public User UserIdFromAccountID(int accountId)
        {
            User userID = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT users.username, users.user_id, '' AS password_hash, '' AS salt FROM users
                                           JOIN accounts ON accounts.user_id = users.user_id
                                           WHERE account_id = @account_id",conn);
                    cmd.Parameters.AddWithValue(@"account_id", accountId);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows && rdr.Read())
                    {
                        userID = GetUserFromReader(rdr);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return userID;

        }

        private Transfer RowToTransfer(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();
            transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
            transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
            transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
            transfer.Amount = Convert.ToDecimal(reader["amount"]);
            transfer.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
            transfer.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
            return transfer;
        }

        private Account RowToAccount(SqlDataReader rdr)
        {
            Account account = new Account();
            account.Account_Id = Convert.ToInt32(rdr["account_id"]);
            account.Balance = Convert.ToDecimal(rdr["balance"]);
            account.User_Id = Convert.ToInt32(rdr["user_id"]);
            return account;
        }
        private User GetUserFromReader(SqlDataReader reader)
        {
            User u = new User()
            {
                UserId = Convert.ToInt32(reader["user_id"]),
                Username = Convert.ToString(reader["username"]),
                PasswordHash = Convert.ToString(reader["password_hash"]),
                Salt = Convert.ToString(reader["salt"]),
            };

            return u;
        }
    }
}
