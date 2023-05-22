using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {
        private readonly UserModel user;
        public ObservableCollection<TaskModel> BackLog { get; set; }
        public ObservableCollection<TaskModel> InProgress { get; set; }
        public ObservableCollection<TaskModel> Done { get; set; }
        private string _boardName;

        public string BoardName
        {
            get => _boardName;
            set
            {
                _boardName = value;
                RaisePropertyChanged("Name");
            }
        }
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                this._id = value;
                RaisePropertyChanged("Id");
            }
        }

        /// <summary>
        /// constructor for board model.
        /// </summary>
        public BoardModel(BackendController controller, UserModel user, (int id, string boardName) board) : base(controller)// This method needs to make the three columns with all Tasks in them
        {
            this.user = user;
            this.BoardName = board.boardName;
            this.Id = board.id;
            BackLog = new ObservableCollection<TaskModel>(controller.getTasksDetails(user.Email, BoardName, 0).
                Select((c, i) => new TaskModel(controller, BoardName, c)));
            InProgress = new ObservableCollection<TaskModel>(controller.getTasksDetails(user.Email, BoardName, 1).
                         Select((c, i) => new TaskModel(controller, BoardName,c)));
            Done = new ObservableCollection<TaskModel>(controller.getTasksDetails(user.Email, BoardName, 2).
                                Select((c, i) => new TaskModel(controller, BoardName, c)));

            BackLog.CollectionChanged += HandleChange;
            InProgress.CollectionChanged += HandleChange;
            Done.CollectionChanged += HandleChange;

        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (TaskModel t in e.OldItems)
                {
                }
            }
        }
    }
}
