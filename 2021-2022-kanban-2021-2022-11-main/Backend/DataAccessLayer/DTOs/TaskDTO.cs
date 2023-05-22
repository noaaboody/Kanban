using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    /// <summary>
    /// A class of Task DTO. 
    /// </summary
    internal class TaskDTO : DTO
    {
        public const string TaskIDColumnName = "ID";
        public const string TaskColumnOrdColumnName = "ColumnOrd";
        public const string TaskBoardIDColumnName = "BoardID";
        public const string TaskTitleColumnName = "Title";
        public const string TaskCreationTimeColumnName = "CreationTime";
        public const string TaskDueDateColumnName = "DueDate";
        public const string TaskDescriptionColumnName = "Description";
        public const string TaskAssigneeColumnName = "Assignee";
        public const string TaskStateColumnName = "State";




        private int _ID;
        private int _ColumnOrd;
        private int _BoardID;
        private string _Title;
        private string _CreationTime;
        private string _DueDate;
        private string _Description;
        private string _Assignee;
        private int _State;


        public int ID { get => _ID; set { _ID = value; _controller.Update(ID,BoardID, TaskIDColumnName, value); } }
        public int ColumnOrd { get => _ColumnOrd; set { _ColumnOrd = value; _controller.Update(ID, BoardID, TaskColumnOrdColumnName, value); } }
        public int BoardID { get => _BoardID; set { _BoardID = value; _controller.Update(ID, BoardID, TaskBoardIDColumnName, value); } }
        public string Title { get => _Title; set { _Title = value; _controller.Update(ID, BoardID, TaskTitleColumnName, value); } }
        public string CreationTime { get => _CreationTime; set { _CreationTime = value; _controller.Update(ID, BoardID, TaskCreationTimeColumnName, value); } }
        public string DueDate { get => _DueDate; set { _DueDate = value; _controller.Update(ID, BoardID, TaskDueDateColumnName, value); } }
        public string Description { get => _Description; set { _Description = value; _controller.Update(ID, BoardID, TaskDescriptionColumnName, value); } }
        public string Assignee { get => _Assignee; set { _Assignee = value; _controller.Update(ID, BoardID, TaskAssigneeColumnName, value); } }
        public int State { get => _State; set { _State = value; _controller.Update(ID, BoardID, TaskStateColumnName, value); } }

        public TaskDTO(int ID, int BoardID, string Title,string creationTime, string dueTime,string description, string assignee,int ColumnOrd, int state) : base(new TaskDTOMapper ())
        {
            _ID= ID;
            _BoardID= BoardID;
            _ColumnOrd= ColumnOrd;
            _Title = Title;
            _CreationTime = creationTime;
            _DueDate = dueTime;
            _Description = description;
            _Assignee = assignee;
            _State = state;

        }


    }

}

