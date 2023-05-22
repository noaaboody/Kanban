using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{

    /// <summary>
    /// A class of the board's actions.
    /// </summary>
    ///
    public class Board
    {
        private string name;
        public int boardID;
        public string owner;
        public List<string> users = new List<string>();
        public Column[] columns;
        private int currentTaskID;
        private BoardDTOMapper mapper;
        private BoardDTO boardDTO;
        private ColumnDTOMapper ColumnMapper;
        private BoardUserDTOMapper boardUserDTOMapper;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly int BackLog;
        readonly int InProgress;
        readonly int Done;




        /// <summary>
        /// This method adds new task to the board.
        /// </summary>
        /// <param name="name">The name of the the.</param>
        /// <param name="email">the email of the user creating the board</param>
        public Board(string name, string email, int boardId)
        {
            this.boardID = boardId;
            name = name.Trim();
            email = email.ToLower().Trim();
            users.Add(email);
            this.owner = email;
            columns = new Column[3];
            for (int i = 0; i < 3; i++)
            {
                columns[i] = new Column(i, boardId);
            }
            currentTaskID = 0;
            this.name = name;
            BackLog = 0;
            InProgress = 1;
            Done = 2;
            mapper = new BoardDTOMapper();  
            ColumnMapper = new ColumnDTOMapper();
            boardUserDTOMapper=new BoardUserDTOMapper();
        }
        
        
        /// <returns>Name of the board  </returns>
        public string getName()
        {
            return name;
        }
        /// <returns> A list of strings containing the emails of the users in the board </returns>
        public List<string> GetUsersOfBoard()
        {
            return users;
        }

        public int GetCurrentTaskId()
        {
            return this.currentTaskID;
        }

        /// <summary>
        /// This method adds new task to the board.
        /// </summary>
        /// <param name="title">The title of the task.</param>
        /// <param name="description">The description of the task, does not necessary</param>
        /// <param name="dueDate">The dueDate of the task</param>
        public void AddTask(string title, string description, DateTime dueDate)
        {
            if(columns[BackLog].getMaxLength() != -1 && columns[BackLog].tasks.Count() >= columns[BackLog].getMaxLength())
            {
                log.Error("no space in backlog coloum");
                throw new Exception("No space in Back Log Column");
            }
            if (title == null || title.Trim().Length == 0)
            {                
                log.Debug("empty task title error");
                throw new Exception("empty task title error");
            }
            if (title.Length > 50)
            {                
                log.Debug("The title is too long");
                throw new Exception("The title is too long");
            }
            if (dueDate < DateTime.Now)
            {
                log.Warn("DateTime is earlier than current time");
                throw new Exception("DateTime is earlier than current time");
            }
            if (description == null)
            {
                description = " ";
            } 
            else if(description.Length > 300)
            {
                log.Debug("The description has more then 300 chars");
                throw new Exception("task descritpion can not be longer than 300 characters");
            }
            Task T = new Task(currentTaskID, title, dueDate, description, this.boardID,0);
            T.InsertToData();
            columns[BackLog].tasks.Add(currentTaskID, T);
            currentTaskID++;
            log.Debug("The task successfully added");
        }

        /// <summary>
        /// This method return task according the ID of the task.
        /// </summary>
        /// <param name="taskID">The ID of the task, unique to tasks in every board</param>
        /// <returns> the Task, unless an error occurs </returns>
        public Task GetTask(int taskID)
        {
            if (columns[BackLog].tasks.ContainsKey(taskID))
            {
                log.Debug("The task in 'backlog' coloum, return the task successfully");
                return columns[BackLog].tasks.GetValueOrDefault(taskID);
            }

            if (columns[InProgress].tasks.ContainsKey(taskID))
            {
                log.Debug("The task in 'in progress' coloum, return the task successfully");
                return columns[InProgress].tasks.GetValueOrDefault(taskID);
            }

            if (columns[Done].tasks.ContainsKey(taskID))
            {
                log.Debug("The task in 'done' coloum, return the task successfully");
                return columns[Done].tasks.GetValueOrDefault(taskID);
            }
            log.Error("Task does not exist, ID illegal");
            throw new Exception("Task does not exist");
        }

        /// <summary>
        /// This method return list of task from 'in progress' coloum.
        /// </summary>
        /// <returns> list of task , unless an error occurs </returns>
        internal List<Task> InProgressTasks()
        {
            List<Task> list = new List<Task>();
            foreach(var task in columns[InProgress].tasks)
            {
                list.Add(task.Value);
            }
            log.Debug("return list of tasks from 'in progress' coloum successfully");
            return list;           
        }

        /// <summary>
        /// This method set the limit of the tasks in each coloum, according his coloum ordinal.
        /// </summary>
        /// <param name="columnOrd">The number of the coloum.
        /// coloum ordinal description - {1:'backlog', 2:'in progress', 3:'done'} </param>
        /// <param name="limit">The number of the tasks in specific coloum</param>
        public void SetColumnLimit(int columnOrd, int limit)
        {

            bool limited = false;
            if (limit != -1 && limit < 0)
            {
                log.Error("column limit must be a positive value");
                throw new Exception("column limit must be a positive value");
            }
            else if (columnOrd == BackLog)
            {
                if (columns[BackLog].tasks.Count() > limit &&limit!=-1)
                {
                    log.Error("column limit must be bigger than amount of tasks in column");
                    throw new Exception("column limit must be bigger than amount of tasks in column");
                }

                log.Debug($"set the limit of 'backlog' coloum to {limit} successfully");
                columns[BackLog].SetColumnLimit(limit);
                limited = true;              
            }
            if(!limited && columnOrd == InProgress)
            {
                if (columns[InProgress].tasks.Count() > limit && limit != -1)
                {
                    log.Error("column limit must be bigger than amount of tasks in column");
                    throw new Exception("column limit must be bigger than amount of tasks in column");
                }

                log.Debug($"set the limit of 'in progress' coloum to {limit} successfully");
                columns[InProgress].SetColumnLimit(limit);
                limited=true;
            }
            if (!limited && columnOrd == Done)
            {
                if (columns[Done].tasks.Count() > limit && limit != -1)
                {
                    log.Error("column limit must be bigger than amount of tasks in column");
                    throw new Exception("column limit must be bigger than amount of tasks in column");
                }

                log.Debug($"set the limit of 'done' coloum to {limit} successfully");
                columns[Done].SetColumnLimit(limit);
                limited = true;
            }
            if (!limited)
            {
                log.Error("Column ordinal must be 0,1 or 2");
                throw new Exception("column ordinal must be 0, 1 or 2");
            }
        }

        
        /// <summary>
        /// This method advance the task to the next colomn.
        /// if the task cannot be advanced an error will occur.
        /// </summary>
        /// <param name="taskId">The ID of the task.
        public void AdvanceTask(string email,int taskId)
        {      
            bool advanced=false;
            log.Info($"advancing task #{taskId} in board {name}");

            if (columns[BackLog].tasks.ContainsKey(taskId))
            {
                if (columns[InProgress].getMaxLength() == -1 || columns[InProgress].tasks.Count() < columns[InProgress].getMaxLength())
                {
                    Task task = columns[BackLog].tasks.GetValueOrDefault(taskId);
                    columns[InProgress].tasks.Add(taskId, task);
                    columns[BackLog].tasks.Remove(taskId);
                    task.AdvanceTask(email);
                    advanced=true;
                    log.Info("task advanced successfully");
                }
                else
                {
                    log.Error($"the task {taskId} cannot be advanced, In progress length is in max Length");
                    throw new Exception("In progress length is in max Length");
                }
            }

            else if (!advanced&&columns[InProgress].tasks.ContainsKey(taskId))
            {
                {
                    if (columns[Done].getMaxLength() == -1 || columns[InProgress].tasks.Count() < columns[Done].getMaxLength())
                    {
                        Task task = columns[InProgress].tasks.GetValueOrDefault(taskId);
                        columns[Done].tasks.Add(taskId, task);
                        columns[InProgress].tasks.Remove(taskId);
                        task.AdvanceTask(email);
                        advanced = true;    
                        log.Debug("the task advanced successfully");

                    }
                    else
                    {
                        log.Error($"the task {taskId} cannot be advanced, Done length is in max Length");
                        throw new Exception("Done length is in max length");
                    }
                }
            }

            else if(!advanced&&columns[Done].tasks.ContainsKey(taskId))
            {
                log.Error($"the task {taskId} cannot be advanced, this task is already done");
                throw new Exception("can not advance tasks that are already done");
            }
            
            if(!advanced)
            { 
                 log.Error($"the task {taskId} cannot be advanced, this task not in this board");
                throw new Exception("can not advance tasks that are not in board");
            }
        }

        /// <summary>
        /// This method return list of tasks in the specific coloum.
        /// </summary>
        /// <param name="columnOrdinal">The number of the coloumn.
        /// coloum ordinal description - {1:'backlog', 2:'in progress', 3:'done'} </param>
        /// <return>list of tasks in the specific coloum</return>
        public List<Task> GetColumn(int columnOrdinal)
        {
            log.Info("get all the task in the specific coloum");
            if (columnOrdinal == BackLog)
            {
                log.Debug("return the tasks of backlog coloum successfully");
                return GetBackLog();
            }
            if (columnOrdinal == InProgress)
            {
                log.Debug("return the tasks of 'in progress' coloum successfully");
                return InProgressColumn();
            }
            if (columnOrdinal == Done)
            {
                log.Debug("return the tasks of done coloum successfully");
                return GetDone();
            }
            log.Error("Column Ordinal must be 0, 1 or 2");
            throw new Exception("Column Ordinal must be 0, 1 or 2");
        }

        /// <summary>
        /// This method return the name of the specific coloum.
        /// </summary>
        /// <param name="columnOrdinal">The number of the coloum.
        /// coloum ordinal description - {1:'backlog', 2:'in progress', 3:'done'} </param>
        /// <return>name of the specific coloum</return>
        public string GetColumnName(int columnOrdinal)
        {
            if (columnOrdinal == BackLog)
            {
                log.Debug("the name of the coloum returned successfully");
                return "Backlog";
            }
            if (columnOrdinal == InProgress)
            {
                log.Debug("the name of the coloum returned successfully");
                return "In progress";
            }
            if (columnOrdinal == Done)
            {
                log.Debug("the name of the coloum returned successfully");
                return "Done";
            }
            log.Error($"the columnOrdinal {columnOrdinal} illegal");
            throw new Exception("Illegal columnOrdinal");
        }

        /// <summary>
        /// This method return the limit of the specific coloum.
        /// </summary>
        /// <param name="columnOrdinal">The number of the coloum.
        /// coloum ordinal description - {1:'backlog', 2:'in progress', 3:'done'} </param>
        /// <return>limit of the specific coloum</return>
        public int GetColumnLimit(int columnOrdinal)
        {
            if (columnOrdinal == BackLog)
            {
                log.Debug("the limit of the coloum returned successfully");
                return columns[BackLog].getMaxLength();
            }
            if (columnOrdinal == InProgress)
            {
                log.Debug("the limit of the coloum returned successfully");
                return columns[InProgress].getMaxLength();
            }
            if (columnOrdinal == Done)
            {
                log.Debug("the limit of the coloum returned successfully");
                return columns[Done].getMaxLength();
            }
            log.Error($"the columnOrdinal { columnOrdinal} illegal");
            throw new Exception("Illegal columOrdial");
        }

        /// <summary>
        /// This method return list of tasks in backlog coloum.
        /// </summary>
        /// <return>list of tasks in the backlog coloum</return>
        private List<Task> GetBackLog() ////OLD FUNC NEED TO DELETE
        {
            List<Task> result = new List<Task>();
            foreach(Task task in columns[BackLog].tasks.Values)
            {
                result.Add(task);
            }
            log.Debug("return list of tasks successfully");
            return result;
        }

        /// <return>list of tasks in the IN progress coloum</return>
        private List<Task> InProgressColumn() 
        {
            List<Task> result = new List<Task>();
            foreach (Task task in columns[InProgress].tasks.Values)
            {
                result.Add(task);
            }
            log.Debug("return list of tasks successfully");
            return result;
        }

     
        /// <return>list of tasks in the 'in progress' coloum who are assigned to a certian user</return>
        public List<Task> GetInProgress(string email) 
        {
            List<Task> result = new List<Task>();
            foreach (Task task in columns[InProgress].tasks.Values)
            {
                if (task.isAssignee(email))
                { result.Add(task); }
            }
            log.Debug("return list of tasks successfully");
            return result;
        }

        /// <summary>
        /// This method return list of tasks in backlog coloum.
        /// </summary>
        /// <return>list of tasks in the done coloum</return>
        private List<Task> GetDone()
        {
            List<Task> result = new List<Task>();
            foreach (Task task in columns[Done].tasks.Values)
            {
                result.Add(task);
            }
            log.Debug("return list of tasks successfully");
            return result;
        }

        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="email">Email of the new owner</param>
        public void changeOwner(string email, string currentOwnerEmail)
        {           
            if (!currentOwnerEmail.Equals(owner))
            {
                log.Debug("The email of the the current owenr is not correct, failed!");
                throw new Exception("The email of the the current owenr is not correct");
            }
            else if (!users.Contains(email))
            {
                log.Debug("new owner is not in this board");
                 throw new Exception("email of new owner is not in using this board");
            }
            else
            {
                owner = email;
                log.Debug("new owner set successfully");
            }
        }
               
        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        public void JoinBoard(string email, int boardID)
        {
            if (!IsValidEmail(email))
            {
                log.Debug("The email of the added user to the board is illegal, faild");
                throw new Exception("The email of the added user to the board is illegal");
            }
            else if (users.Contains(email))
            {
                log.Debug("The user is alredy in using the board, faild");
                throw new Exception("The user is alredy in using the board");
            }
            
            else
            {
                users.Add(email);
                log.Debug("added use successfully");

            }

        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        public void RemoveUser(string email, int boardID)
        {
            if (!IsValidEmail(email))
            {
                log.Debug("The email of the  user is illegal, faild");
                throw new Exception("The email user to the board is illegal");
            }
            if (owner.Equals(email))
            {
                log.Debug("owner cannot leave board");
                throw new Exception("owner cannot leave board");
            }
            else if (!users.Contains(email))
            {
                log.Debug("The user is  not in the board, faild");
                throw new Exception("The user is alredy not using the board");
            }
            else
            {
                foreach (Task t in columns[BackLog].tasks.Values)
                {
                    if (t.isAssignee(email))
                        t.removeAsignee(email);
                }
                foreach (Task t in columns[InProgress].tasks.Values)
                {
                    if (t.isAssignee(email))
                        t.removeAsignee(email);
                }
                users.Remove(email);
                log.Debug("user removed successfully");

            }
        }
             
        /// <summary>
        /// method used for validating proper Email form
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsValidEmail(string email)
        {
            Regex rx = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex rx2 = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            Regex rx3 = new Regex(@"^\w+([.-]?\w+)@\w+([.-]?\w+)(.\w{2,3})+$");
            Regex rx4 = new Regex(@"^[a-zA-Z0-9_!#$%&'*+/=?`{|}~^.-]+@[a-zA-Z0-9.-]+$");

            if (!(rx.IsMatch(email)) | !(rx2.IsMatch(email)) | !(rx3.IsMatch(email)) | !(rx4.IsMatch(email)))
            {
                return false;
            }
            email = email.Trim();
            if (email.EndsWith("."))
                return false;
            return true;

        }

        /// <summary>
        /// method used Inserting the board only if it a new board and not loaded
        /// </summary>
        

        /// <summary>
        /// method used to load all the data in to the board from dal
        /// </summary>
        public void LoadData()
        {
            List<BoardUserDTO> usersDto = boardUserDTOMapper.SelectAllBoardUsers();
            List<TaskDTO> tasks = (new TaskDTOMapper()).SelectAllTasks();
            List<ColumnDTO> columnsdtoList = ColumnMapper.SelectAllColumns();
            int max = 0;

            foreach (ColumnDTO column in columnsdtoList)
            {
                if (column.BoardID == boardID)
                {
                    this.columns[column.ColumnOrd].SetColumnLimit(column.MaxLength);
                }
            }
            foreach (BoardUserDTO userD in usersDto)
            {
                if (userD.BoardID == this.boardID && !userD.Email.Equals(owner))
                {
                    users.Add(userD.Email);
                }

            }

            foreach (var task in tasks)
            {
                Task temp = new Task(task.ID, task.Title, Convert.ToDateTime(task.DueDate), task.Description, task.BoardID, task.ColumnOrd);
                if (task.BoardID == this.boardID)
                {
                    if(task.ID==max)
                        max = task.ID;
                    if (task.ColumnOrd == 0)
                    {
                        this.columns[BackLog].tasks.Add(task.ID, temp);
                    }
                    else if (task.ColumnOrd == 1)
                    {
                        this.columns[InProgress].tasks.Add(task.ID, temp);
                    }
                    else if (task.ColumnOrd == 2)
                    {
                        this.columns[Done].tasks.Add(task.ID, temp);
                    }

                    else
                    {
                        log.Debug("Data is incorrect");
                    }
                }
 
            }
            max += 1;
            currentTaskID = max;
        }
    }
}
