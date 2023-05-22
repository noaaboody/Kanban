using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using System.Text.Json;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    /// <summary>
    /// Class represents an individual User in the system
    /// </summary>
    public class User
    {
        public string email { get; private set; }
        public string password { get; private set; }
        public bool connected { get; private set; }
       
       
        
        /// <summary>
        /// User have an email a password, each user created in the system is not connected by default
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password"> User password</param>
        public User(string email, string password)
        {
            this.email = email;
            this.password = password;
            connected = false;                       
        }
        /// <summary>
        /// returns User email
        /// </summary>
        /// <returns></returns>
        public string getEmail() { return email; }
        /// <summary>
        /// returns User Password
        /// </summary>
        /// <returns></returns>
        public string getPassword() { return password; }
       /// <summary>
       /// sets User password to a new password
       /// </summary>
       /// <param name="password">new password</param>
        public void setPassword(string password) { this.password = password; }

        /// <summary>
        ///  This method returns true if the user is logged in.
        /// </summary>
        /// <returns>true if the user is logged in, else false</returns>
        public bool isConnected() { return connected; }

        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <returns>string with the user's email, unless an error occurs</returns>
        public String LogIn()
        {
            connected = true;
            return email;
        }

        /// <summary>
        /// This method logs out a user. 
        /// </summary>
        public void LogOut() { connected = false;}       
    }
}
