using MenuFramework;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using TenmoServer.Models;
using TenmoServer.DAO;
using TenmoClient.Data;

namespace TenmoClient.Views
{
    public class MainMenu : ConsoleMenu
    {
        private AccountSqlDAO accountDAO;
        private UserSqlDAO userDAO;

        public MainMenu()
        { 
            AddOption("View your current balance", ViewBalance)
                .AddOption("View your past transfers", ViewTransfers)
                //.AddOption("View your pending requests", ViewRequests)
                .AddOption("Send TE bucks", SendTEBucks)
                //.AddOption("Request TE bucks", RequestTEBucks)
                .AddOption("Log in as different user", Logout)
                .AddOption("Exit", Exit);
        }

        protected override void OnBeforeShow()
        {
            Console.WriteLine($"TE Account Menu for User: {UserService.GetUserName()}");
        }

        private MenuOptionResult ViewBalance()
        {
            AuthService authService = new AuthService();
            decimal balance = authService.GetBalance();
            Console.WriteLine($"Your current account balance is: {balance:c}");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        //$"acccounts/transfers/{transferId}
        // "acccounts/transfers"
        private MenuOptionResult ViewTransfers()
        {
            Transfer transfer = new Transfer();
            AuthService authService = new AuthService();
            int userId = UserService.GetUserId();
            List<Transfer> transfers = authService.ViewAllTransfers();
            List<API_User> users = authService.ListofAvailableUsers();
            
            foreach(API_User user in users)
            {
                if(userId == user.UserId)
                {
                    
                }
            }
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Transfer");
            Console.WriteLine("ID\tFrom/To\t\t\tAmount");
            Console.WriteLine("-------------------------------------------");
            string signInUser = UserService.GetUserName();
            foreach (Transfer transfer1 in transfers)
            {
                string displayName;
                //Make method that returns username from their accountId
                //Call method for accountFrom
                //Call method for accountTo
                displayName = (userId == transfer1.AccountTo) ? $"From:\t{authService.UserNameFromAccountId(transfer1.AccountFrom).Username}" : $"To:\t{authService.UserNameFromAccountId(transfer1.AccountTo).Username}";
                Console.WriteLine($"{transfer1.TransferId}\t{displayName}\t\t{transfer1.Amount}");


            }
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine();
            Console.Write("Please enter transfer ID to view details (0 to cancel): ");
            string input = Console.ReadLine();
            int transferId = Convert.ToInt32(input);
            if (transferId == 0)
            {
                return MenuOptionResult.DoNotWaitAfterMenuSelection;
            }
            
            bool isTransferListed = false;
            foreach (Transfer transfer1 in transfers)
            {
                if (transfer1.TransferId == transferId)
                {
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Transfer Details");
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine();
                    Console.WriteLine($"Id: {transfer1.TransferId}");
                    Console.WriteLine($"From: {authService.UserNameFromAccountId(transfer1.AccountFrom).Username}");
                    Console.WriteLine($"To: {authService.UserNameFromAccountId(transfer1.AccountTo).Username}");
                    string type;
                    type = (userId == transfer1.AccountTo) ? "Receive" : "Send";
                    Console.WriteLine($"Type: {type}");
                    Console.WriteLine($"Status: Approved");
                    Console.WriteLine($"Amount: ${transfer1.Amount}");
                    isTransferListed = true;
                }
            }
            if (!isTransferListed)
            {
                Console.WriteLine("The ID you entered was not valid!");
            }

            return MenuOptionResult.WaitAfterMenuSelection;
        }

        //private MenuOptionResult ViewRequests()
        //{
        //    Console.WriteLine("Not yet implemented!");
        //    return MenuOptionResult.WaitAfterMenuSelection;
        //}

        private MenuOptionResult SendTEBucks()
        {
            //pull in Transfer
            Transfer transfer = new Transfer();
            // pull in authservice
            AuthService authService = new AuthService();
            // get list of users ids
            List<API_User> list = authService.ListofAvailableUsers();

            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("\tUsers");
            Console.WriteLine("\tID \t\t Name");
            Console.WriteLine("--------------------------------------------");

            foreach (API_User userList in list)
            {

                Console.WriteLine($"\t{userList.UserId} \t\t {userList.Username}");
            }
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine();

            //pull transfer id info and convert user input to an int
            Console.WriteLine("Enter the ID of the user you would like to send some TEBucks to (0 to cancel)");
            int userInput = Convert.ToInt32(Console.ReadLine());
            if (userInput == 0)
            {
                Console.WriteLine("Transaction Canceled!");
                Console.WriteLine("Please hit 'Enter' to contiune.");
                return MenuOptionResult.WaitAfterMenuSelection;
            }
            // userInput doesnt equal User.UserID possible contains
            if (userInput == UserService.GetUserId())
            {
                Console.WriteLine("You can not send money to yourself. Please use the correct user ID");
                Console.WriteLine("Hit 'Enter' to return to the main menu");
                return MenuOptionResult.WaitAfterMenuSelection;

            }
            transfer.AccountTo = userInput;
            transfer.AccountFrom = UserService.GetUserId();
            Console.WriteLine("Please enter the amount you would like to transfer");
            
            transfer.Amount = Convert.ToDecimal(Console.ReadLine());
            
           
            
            if(transfer.Amount > authService.GetBalance())
            {
                Console.WriteLine("You have an insufficient balance");
            }
            else
            {
                Console.WriteLine("Transactions was completed successful!");
            }
            authService.Transfer(transfer);
            return MenuOptionResult.WaitAfterMenuSelection;
           
            
        }

        //private MenuOptionResult RequestTEBucks()
        //{
        //    Console.WriteLine("Not yet implemented!");
        //    return MenuOptionResult.WaitAfterMenuSelection;
        //}

        private MenuOptionResult Logout()
        {
            UserService.SetLogin(new API_User()); //wipe out previous login info
            return MenuOptionResult.CloseMenuAfterSelection;
        }

    }
}
