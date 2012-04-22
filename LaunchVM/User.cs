using System;
using System.Collections.Generic;
using System.Linq;

namespace LaunchVM
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Logout { get; set; }

        /// <summary>
        /// Initializes a new instance of the User class.
        /// </summary>
        public User()
        {
            Logout = true;
        }

        /// <summary>
        /// Initializes a new instance of the User class.
        /// </summary>
        public User(string userName, string password)
            : this()
        {
            UserName = userName;
            Password = password;
        }

    }
}
