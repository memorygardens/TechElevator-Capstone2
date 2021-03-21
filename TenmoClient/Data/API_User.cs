﻿namespace TenmoClient.Data
{
    /// <summary>
    /// Return value from login endpoint
    /// </summary>
    public class API_User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// Model to provide login parameters
    /// </summary>
    public class LoginUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
