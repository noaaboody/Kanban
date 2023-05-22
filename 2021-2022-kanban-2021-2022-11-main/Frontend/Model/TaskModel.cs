using IntroSE.Kanban.Backend.ServiceLayer;
using System;

namespace Frontend.Model
{
    public class TaskModel : NotifiableModelObject
    {
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
        private string _title;
        public string Title
        {
            get => "Title: "+_title;
            set
            {
                this._title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string _description;
        public string Description
        {
            get => "Description: " + _description;
            set
            {
                this._description = value;
                RaisePropertyChanged("Description");
            }
        }
        private DateTime _creationTime;
        public DateTime CreationTime
        {
            get => _creationTime;
            set
            {
                this._creationTime = value;
                RaisePropertyChanged("CreationTime");
            }
        }
        private DateTime _dueDate;
        public DateTime DueDate
        {
            get =>  _dueDate;
            set
            {
                this._dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }
        /// <summary>
        /// constructor for task model.
        /// </summary>

        public TaskModel(BackendController controller, string boardName, TaskDetailsModel details) : base(controller)
        {
            Id = details.Id;
            CreationTime = details.CreationTime;
            Title = details.Title;
            Description = details.Description;
            DueDate = details.DueDate;            
        }

        /// <summary>
        /// class meant to simplifiy extracting Task details from the backend layer
        /// </summary>
        public class TaskDetailsModel
        {
            public int Id { get; set; }
            public DateTime CreationTime { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime DueDate { get; set; }
        }
    }

}
