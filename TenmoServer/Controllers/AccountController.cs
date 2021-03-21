using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("/accounts")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private IAccountDAO accountDAO;
        private IUserDAO userDAO;

        public AccountController(IAccountDAO accountDAO, IUserDAO userDAO)
        {
            this.accountDAO = accountDAO;
            this.userDAO = userDAO;
        }

        [HttpGet("users")]
        public ActionResult<Account> GetAccount()
        {
            int userId = Convert.ToInt32(User.FindFirst("sub")?.Value);
            Account account = accountDAO.GetAccount(userId);
            if (account == null)
            {
                return NotFound();
            }
            else
            {
                return account;
            }
        }

        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            List<User> users = userDAO.GetUsers();
            if (users == null)
            {
                return NotFound();
            }
            else
            {
                return users;
            }
        }

        [HttpGet("{accountId}/users")]
        public ActionResult<User> GetUsers(int accountId)
        {
            User users = accountDAO.UserIdFromAccountID(accountId);
            
            if (users == null)
            {
                return NotFound();
            }
            else
            {
                return users;
            }
        }

        [HttpGet("transfers")]
        public ActionResult<List<Transfer>> GetTransfers()
        {
            int userId = Convert.ToInt32(User.FindFirst("sub")?.Value);
            List<Transfer> transfers = accountDAO.GetTransfers(userId);
            if (transfers == null)
            {
                return NotFound();
            }
            else
            {
                return transfers;
            }
        }

        [HttpPut]
        public ActionResult Transfer(Transfer transfer)
        {
            int userId = transfer.AccountFrom;
            int accountToId = transfer.AccountTo;
            decimal amount = transfer.Amount;

            accountDAO.TransferToRegUser(userId, accountToId, amount);
            return Ok();
        }
    }
}
