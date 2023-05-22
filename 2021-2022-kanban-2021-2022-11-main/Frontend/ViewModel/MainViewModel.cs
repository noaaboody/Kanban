using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class MainViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                this._email = value;
                RaisePropertyChanged("Username");
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                this._password = value;
                RaisePropertyChanged("Password");
            }
        }

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        public MainViewModel()
        {
            this.Controller = new BackendController();
      
        }



        /// <summary>
        /// This method loggs In the user.
        /// </summary>
        /// <returns>UserModel, unless an error occurs</returns>
        public UserModel Login()
        {
           Message = "";
            try
            {
                return Controller.Login(Email, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
               return null;
            }
        }

        /// <summary>
        /// This method registers a new user.
        /// </summary>
        /// <returns>UserModel, unless an error occurs</returns>
        public UserModel Register()
        {
            Message = "";
            try
            {
                UserModel u= Controller.Register(Email, Password);
                Message = "Registered successfully";
                return u;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }


    }
}
