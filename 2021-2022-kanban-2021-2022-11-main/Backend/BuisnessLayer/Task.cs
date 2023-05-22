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
using System.Text.Json.Serialization;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{

    /// <summary>
    /// A class of the task's actions.
    /// </summary>
    public class Task
    {
        public int Id { get; private set; }
        public DateTime CreationTime { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime DueDate{ get; private set; }
        [JsonIgnore]
        public string assignee { get; private set; }
        [JsonIgnore]
        public int state { get; private set; }
        [JsonIgnore]
        public int BoardID { get; private set; }
        private TaskDTO taskDTO;
        private TaskDTOMapper taskMapper;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public Task(int taskId, string title,  DateTime dueTime, string description, int boardID, int columnOrd) 
        {
            this.state = columnOrd;
            this.Id = taskId;
            this.CreationTime = DateTime.Now;
            this.Title = title;
            this.DueDate = dueTime;
            this.Description = description;
            this.assignee = "";
            this.BoardID = boardID;
            this.state = 0;
            this.taskDTO = new TaskDTO(taskId,boardID,title,CreationTime.ToString(),dueTime.ToString(),description,assignee, columnOrd, state);
            this.taskMapper = new TaskDTOMapper();           
        }

        /// <summary>
        /// this method inserts the task to the DataBase
        /// </summary>
        public void InsertToData(){ taskMapper.Insert(taskDTO);}

        /// <summary>
        /// This method change the title.
        /// </summary>
        /// <param name="TaskID">The ID of the task.</param>
        ///<param name="email">The email of the user requesting to edit the task.</param>
        /// <param name="newTitle">The new title of the task.</param>
        /// <param name="BoardID">The ID of the board.</param>
        /// </exception>
        /// <exception cref="The title{title} has more then 50 chars or is empty">
        /// Thrown when the length of the title is not valid.
        /// </exception>
        public void editTitle(string email ,int TaskID, string newTitle, int BoardID)
        {
            if (email.Equals(assignee))
            {
                log.Info("Start changing title");
                email = email.ToLower();
                if (this.state == 2)
                {
                    log.Error("task is already done therfore cannot be editted");
                    throw new Exception("task is akready done");
                }
                if (!isAssignee(email))
                {
                    log.Error("Only asignee can edit tasks");
                    throw new Exception("email is not asignee");
                }
                if (IsValidTitle(newTitle))
                {
                    Title = newTitle;
                    taskMapper.Update(TaskID, BoardID, "Title", newTitle);
                    log.Info("Title update succesfuly");
                }
                else
                {
                    log.Error("new title illigal");
                    throw new Exception($"The title{newTitle} has more then 50 chars or is empty");
                }
            }
            else { throw new Exception("Only asignee can edit task"); }
        }

        /// <summary>
        /// This method check if the title is valid.
        /// </summary>
        /// <param name="title">The name of the task.</param>
        /// <returns> true, if the title is not valid returns false </returns>
        private bool IsValidTitle(string title)
        {
            bool isValidTitle = true;
            if (title == null || title.Trim().Length == 0)
            {
                isValidTitle = false;
                log.Error("empty task title error");
                throw new Exception("empty task title error");
            }
            else if (title.Length > 50)
            {
                isValidTitle = false;
                log.Error("The title is too long");
                throw new Exception("The title is too long");
            }
            log.Info("The title is valid");
            return isValidTitle;
        }

        /// <summary>
        /// This method edit the description.
        /// </summary>
        ///<param name="email">The email of the user requesting to edit the task.</param>
        /// <param name="taskID">The ID of the task.</param>
        /// <param name="description">The new description of the task.</param>
        /// <param name="BoardID">The ID of the board.</param>
        /// </exception>
        /// <exception cref="The description has more then 300 chars">
        /// Thrown when the description is too long.
        /// </exception>
        public void editDescription(string email, int taskID, string description, int BoardID)
        {
            if (assignee.Equals(email))
            {
                log.Info("Start changing description");
                email = email.ToLower();
                if (this.state == 2)
                {
                    log.Error("task is already done therfore cannot be editted");
                    throw new Exception("task is akready done");
                }
                if (!isAssignee(email))
                {
                    log.Error("Only asignee can edit tasks");
                    throw new Exception("email is not asignee");
                }
                if (description.Trim().Length == 0)
                {
                    description = " ";
                }
                if (this.IsValidDescription(description))
                {
                    this.Description = description;
                    taskMapper.Update(taskID, "Description", description);
                    log.Info("the description updated");
                }
                else
                {
                    log.Error("the description is not valid");
                    throw new Exception("The description has more then 300 chars");
                }
            }
            else { throw new Exception("Only asignee can edit task"); }
        }
        /// <summary>
        /// This method check if the description is valid.
        /// </summary>
        /// <param name="description">Thed description of the task.</param>
        /// <returns> true, if the description is not valid returns false </returns>
        private bool IsValidDescription(string description)
        {
            bool isValidDescription = true;
            if (description != null && description.Length > 300)
            {
                isValidDescription = false;
                log.Error("The description has more then 300 chars");
            }
            if (description == null)
            {
                isValidDescription=false;
                log.Error("The description is null");
            }
            return isValidDescription;
            log.Info("Title description succesfuly");
        }

        /// <summary>
        /// Removes the task from the DataBase
        /// </summary>
        public void DeleteTask()
        {
            taskMapper.Delete(this.taskDTO);
        }

        /// <summary>
        /// This method edit the dueDate.
        /// </summary>
        ///<param name="email">The email of the user requesting to edit the task.</param>
        /// <param name="taskID">The ID of the task.</param>
        /// <param name="date">The new dueDate of the task.</param>
        /// <param name="BoardID">The ID of the board.</param>
        public void editDueDate(string email, int taskID, DateTime date, int BoardID) 
        {
            if (assignee.Equals(email))
            {
                log.Info("Start changing dueDate"); email = email.ToLower();
                if (this.state == 2)
                {
                    log.Error("task is already done therfore cannot be editted");
                    throw new Exception("task is akready done");
                }
                if (!isAssignee(email))
                {
                    log.Error("Only asignee can edit tasks");
                    throw new Exception("email is not asignee");
                }

                if (date < DateTime.Now)
                {
                    log.Error("The duedate illegal");
                    throw new Exception("New duedate illegal");
                }
                this.DueDate = date;
                bool succsesToUpdate = taskMapper.Update(taskID, BoardID, "DueDate", date.ToString());
                if (!succsesToUpdate)
                {
                    log.Error("SQL Exeption didn't update DB");
                    throw new Exception("SQL Exeption didn't update DB");
                }
                log.Info("The duedate updated");
            }
            else { throw new Exception("Only asignee can edit task"); }
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>      
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <param name="BoardID">The ID of the board.</param>
        public void AssignTask(string email, string emailAssignee, int BoardID)
        {

            log.Info("Start to assign task"); 
            email = email.ToLower();
            emailAssignee = emailAssignee.ToLower();
            if (this.state == 2)
            {
                log.Error("task is already done therfore cannot be editted");
                throw new Exception("task is akready done");
            }
            if (emailAssignee == this.assignee)
            {
                log.Error("this user already assigned to this task");
                throw new Exception("This User is already aasigned to this task");
            }
            else if (email != assignee && assignee != null)
            {
                log.Error("Only assignee can change the assignee");
                throw new Exception("Only assignee can change the assignee");
            }
            emailAssignee = emailAssignee.ToLower().Trim();
            this.assignee = emailAssignee;
            taskMapper.Update(Id,BoardID, "Assignee", emailAssignee);
            log.Info("the assignee succesfuly added");
        }

        /// <summary>
        /// This method remove the assignee.
        /// </summary>
        /// <param name="emailAssignee">Email of the asignee user</param>
        public void removeAsignee(string emailAssignee)            
        {
            log.Info("Start remove assignee");
            emailAssignee = emailAssignee.ToLower();
            if(emailAssignee.Equals(this.assignee))
            {
                this.assignee = "";
                taskMapper.Update(Id, "Assignee", "");
                log.Info("The assignee succssesfully removed");
            }
            log.Error("Only assignee can change the assignee");
            throw new Exception("Only assignee can change the assignee");
        }

        /// <summary>
        /// This method retruns true if someone is assigned.
        /// </summary>
        /// <returns> true if someone is assigned, else, returns false </returns>
        public bool isAssigned() { return this.assignee!=null; }

        /// <summary>
        /// This method return true if User is the assignee
        /// </summary>
        /// <param name="email">Email of the user.</param>      
        /// <returns> true if the user is the assignee, else, returns false </returns>
 
        public bool isAssignee(string email) {
            email = email.ToLower();
            return email.Equals(assignee);
        }
        

        /// <summary>
        /// method used for advancing a task state in the progress
        /// </summary>
        /// <param name="email">The email of the User</param>
        public void AdvanceTask(string email)
        {
            email = email.ToLower();
            if (state < 2)
            {
                if (isAssignee(email))
                {
                    state++;
                    taskMapper.Update(this.Id, this.BoardID, "ColumnOrd", state);
                    taskMapper.Update(this.Id, this.BoardID, "State", state);
                }
                else
                {
                    log.Error("Only assignee can change the task state");
                    throw new Exception("Only assignee can change the task");
                }
            }
            else
            {
                log.Error("task is already done");
                throw new Exception("task is already done");
            }
        }
    }



}
