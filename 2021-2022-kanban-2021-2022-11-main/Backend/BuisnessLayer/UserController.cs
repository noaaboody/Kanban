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
using System.Text.RegularExpressions;
using System.Globalization;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;


namespace IntroSE.Kanban.Backend.BuisnessLayer
{/// <summary>
/// Class used for maintaining Users and actions related to them in the system
/// </summary>
    public class UserController
    {
        private readonly Dictionary<string, User> _users = new Dictionary<string, User>();
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private UserDTOMapper mapper = new UserDTOMapper();
        private readonly int minPasswordLength = 6;
        private readonly int maxPasswordLength = 20;
/// <summary>
/// this method returns the Users Dictionary created in this class
/// </summary>
/// <returns>Dicitonary <User.email(string), User(BusinessLayer.User)</returns>
        public Dictionary<string, User> GetAllUsers()
        {
            return _users;
        }

        /// <summary>
        /// User registration method, requires a proper Email, and a Password
        /// By default each user being registered to the system is being LoggedIn.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// 
        internal void Register(string email, string password)
        {         
            if (email is null)
            {
                log.Error("Email is null");
                throw new Exception("Email is null");
            }
            email = email.ToLower().Trim();
            if (!IsValidEmail(email))
            {
                log.Warn("Illegal email declined");
                throw new Exception($"Email {email} contains an illegal form");
            }
            if (_users.ContainsKey(email))
            {
                log.Warn($"Email {email} already exists");
                throw new Exception($"Email {email} already exists");
            }
          
            if (!IsValidPassword(password))
            {           
                log.Error($"Password {password} must contain 6-20 letters " +
                        "at least one capital letter, and at least one small letter");
                throw new Exception("password must contain 6-20 letters \n" +
                        "at least one capital letter, and at least one small letter");
            }
            else
            {
                User u = new User(email, password);
                _users.Add(email, u); //ading to dictionary of existing users
                _users[email].LogIn();
                mapper.Insert(new UserDTO(email, password));
                log.Info($"Email {email} and password {password} are valid");
            }
            log.Info("User registered successfully");            
        }

        /// <summary>
        /// method used for logging in users
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal string LogIn(string email, string password)
        {
            if (email is null || password is null)
            { 
                log.Error($"Email {email} or Password {password} null");
                throw new Exception("cannot accept null values");
            }
            email = email.ToLower().Trim();
            if (!_users.ContainsKey(email))
            {
                log.Warn($"Email {email} does not exist");
                throw new Exception($"Email{email} is not registered");
            }
            if (_users[email].getPassword() != password)
            {
                log.Warn($"Incorrect Password {password}");
                throw new Exception("Incorrect password");
            }
            if (_users[email].isConnected())
            {
                log.Error($"user {email} already logged in");
                throw new Exception($"user {email} already logged in");
            }
            _users[email].LogIn();
            log.Info($"User {email} logged in successfully");
             return email;
        }

        /// <summary>
        /// This method loggs out users from the system
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal void LogOut(string email)
        {
            email = email.ToLower().Trim();
            log.Debug($"logOut user {email}");
            PerformOperation(email);
            log.Debug($"User {email} logged out successfully");         
            _users[email].LogOut();           
        }

        /// <summary>
        /// This method checks if the password is valid. 
        /// </summary>
        /// <param name="Password">The password of the user.</param>
        /// <returns>if the password meets the system coditions</returns>
        private bool IsValidPassword(string password)
        {
            bool containsUpper = false;
            bool containsLower = false;
            bool containsNum = false;           
            int passLength = password.Length;
            if (passLength >= minPasswordLength && passLength <= maxPasswordLength)
            {
                foreach (char c in password)
                {
                    if (c >= 'a' && c <= 'z')
                        containsLower = true;
                    else if (c >= 'A' && c <= 'Z')
                        containsUpper = true;
                    else if(c >= '0' && c <= '9')
                        containsNum = true;
                    if (containsLower && containsUpper && containsNum)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// method used for validating proper Email form
        /// </summary>
        /// <param name="email"></param>
        /// <returns>if email is valid</returns>       
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Method used for changing an existing user's password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal void ChangePassword(string email, string password)
        {
            log.Debug($"Change user's {email} password to {password}");
            PerformOperation(email);
            if (password == null || password.Length == 0)
            {
                log.Error($"Password is null");
                throw new Exception("New Password can not be null or empty");
            }
            if (!IsValidPassword(password))
            {
                log.Warn($"Password {password} must contain 6-20 letters, " +
                     "at least one uppercase letter, and at least one lowercase letter");
                throw new Exception("password must contain 6-20 characters" +
                    ", at least one uppercase letter and at least one lowercase letter");
            }
            log.Debug("The password was successfully changed");
            _users[email].setPassword(password);
            mapper.Update(email,"Password", password);
        }

      
  
        /// <summary>
        /// method used to get a User as an object from the Users Dictionary.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public User GetUser(string email)
        {
            if (email == null)
            {
                log.Debug($"email is null");
                throw new Exception("email is null");
            }
            email = email.ToLower().Trim();           
            if (!_users.ContainsKey(email))
            {
                log.Debug($"email {email} does not exist");
                throw new Exception($"email {email} does not exist");
            }
            log.Debug($"returned user successfully");
            return _users[email];
        }
        /// <summary>
        /// Public method used to validate whether a system operation can be performed on a specific user
        /// to perform an operation the user email must be legal, belong to an existing logged in user.
        /// </summary>
        /// <param name="email">email represents a user email</param>
        /// <exception cref="Exception">throws an exception according to the codition that the email did not met</exception>
    public void PerformOperation(string email)
        {
            if(email is null)
            {
                log.Debug("null email accepted");
                throw new Exception("email is null");
            }
            email = email.ToLower().Trim();
            if (!_users.ContainsKey(email))
            {
                log.Debug($"email {email} does not exist");
                throw new Exception($"email {email} does not exist");
            }
            if (!_users[email].isConnected())
            {
                log.Debug($"email {email} is not logged in");
                throw new Exception($"email {email} is not logged in");
            }
        }
        /// <summary>
        /// method used for knowing if a user exists on the system
        /// </summary>
        /// <param name="email"></param>        
        public void isUserExists(string email)
        {
            log.Info("checking if user {email} exists in the system");
            if (email == null) 
            {
                log.Warn("null email accepted");
                throw new Exception("null email");
            }               
            email = email.ToLower().Trim();           
            bool exists =  _users.ContainsKey(email);
            if (!exists)
            {
                log.Warn($"email {email} does not exits in the system");
                throw new Exception("this user does not exits in the system");
            }
        }

        /// <summary>
        /// Method used to load all Users from the DAL
        /// </summary>
        internal void LoadAllUsers()
        {
            log.Info("loading all users from DAL");
            List<UserDTO> users = mapper.SelectAllUsers();
            users.ForEach(u => _users.Add(u.Email, new User(u.Email,u.Password)));
        }

        /// <summary>
        /// Method used to Delete all Users Data
        /// </summary>
        internal void DeleteAllUsers()
        {
            log.Info("deleting all users from the data base");
            mapper.DeleteAllData();
            _users.Clear();
        }

    }
}
