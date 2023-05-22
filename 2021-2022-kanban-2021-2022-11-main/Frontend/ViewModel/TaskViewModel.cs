using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    public class TaskViewModel : NotifiableObject
    {
        private Model.BackendController controller;
        public BoardModel board { get; private set; }

        private bool _enableForward = false;
        public string Title { get; private set; }
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
        /// constructor for the model
        /// </summary>
        /// <returns>Constructs, unless an error occurs</returns>
        public TaskViewModel(BoardModel board)
        {
            this.controller = board.Controller;
            this.board = board;
            Title = "Tasks of: " + board.BoardName;           
        }



    }
}
