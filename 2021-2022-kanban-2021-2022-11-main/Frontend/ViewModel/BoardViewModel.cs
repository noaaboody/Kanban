using Frontend.Model;
using System;
using System.Windows;
using System.Windows.Media;

namespace Frontend.ViewModel

{
    internal class BoardViewModel : NotifiableObject
    {
        private BackendController controller;
        public UserModel user { get; private set; }    
        public string Title { get; private set; }
        private BoardModel _selectedBoard;
        public BoardModel SelectedBoard
        {
            get
            {
                return _selectedBoard;
            }
            set
            {
                _selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedBoard");
            }
        }
        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }

        /// <summary>
        /// This method constructs a board view model.
        /// </summary>
        public BoardViewModel(UserModel user)
        {
            this.controller = user.Controller;
            this.user = user;
            Title = "Boards of: " + user.Email;
        }

        public SolidColorBrush BackgroundColor
        {
            get
            {
                return new SolidColorBrush(user.Email.Contains("eldor") ? Colors.Blue : Colors.LightSlateGray);
            }
        }



    }
}
