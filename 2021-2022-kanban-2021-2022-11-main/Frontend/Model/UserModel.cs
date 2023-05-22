using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Frontend.Model
{
    public class UserModel : NotifiableModelObject
    {
        public BackendController BackendController { get; set; }

        private string _email;      
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }
        public ObservableCollection<BoardModel> Boards { get; set; }

        private UserModel(BackendController controller, ObservableCollection<BoardModel> boards) : base(controller)
        {
            Boards = boards;
            Boards.CollectionChanged += HandleChange;
        }

        /// <summary>
        /// contructor for user model
        /// </summary>
        public UserModel(BackendController controller, string email) : base(controller)
        {
            this.Email = email;
            Boards = new ObservableCollection<BoardModel>(controller.GetUserBoardIDs(email).
                Select((c, i) => new BoardModel(controller, this, controller.GetBoardsName(i))).ToList());           
            Boards.CollectionChanged += HandleChange;
        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
            }
        }
    }
}

