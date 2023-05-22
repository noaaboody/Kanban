using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    /// <summary>
    /// A class of board DTO. 
    /// </summary
    internal class UserDTO :DTO 
    {
        public const string UserEmail = "Email";
        public const string UserPassword = "Password";
      
        private string _Email;
        private string _Password;
        
       /// <summary>
       /// represents User Email, simple getter.
       /// </summary>
       /// 
        //public string Email { get => _Email; set { _Email = value; _controller.Update(Email, UserEmail, value); } }
        public string Email { get => _Email;}

        /// <summary>
        /// represents User Password, simple getter, setter updates User's password on Users table
        /// </summary>
        public string Password { get => _Password; set { _Password = value; _controller.Update(Email, UserPassword, value); } }
       


        public UserDTO(string Email, string Password) : base(new UserDTOMapper())
        {
            _Email = Email;
            _Password = Password;   
        }
    }

}

