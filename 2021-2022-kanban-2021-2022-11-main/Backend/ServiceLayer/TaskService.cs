using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.ServiceLayer
{

        /// <summary>
        /// A class of the task's action on the board. 
        /// <para>
        /// Each of the class' methods should return a string.
        /// </para>
        /// </summary>

    public class TaskService

    {
        private readonly UserController users;
        private readonly BoardController boardController;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TaskService(UserController controller, BoardController boards)
        {
            this.users = controller;
            this.boardController = boards;
            // Load configuration
            //Right click on log4net.config file and choose Properties. 
            //Then change option under Copy to Output Directory build action into Copy if newer or Copy always.
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log!");
        }


        /// <summary>
        /// This method creating new task.
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="boardName">The name of the task's board</param>
        /// <param name="title">The title of the task.</param>
        /// <param name="description">The description of the task, does not necessary</param>
        /// <param name="dueDate">The dueDate of the task</param>
        public string createTask(string email, string boardName, string title, string description, DateTime dueDate) 
        {
            Response r = new Response();
            try
            {
                int boardId = boardController.GetBoardIdByName(email, boardName);
                boardController.GetBoard(boardId).AddTask(title, description, dueDate);
                log.Debug("Task created");              
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                r = new Response(e.Message);                
            }
            return r.ToJson();
        }


        /// <summary>
        /// This method update the due date of the task.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in.</param>
        /// <param name="boardName">The name of the board of the task.</param>
        /// <param name="taskID">The number of the task.</param>
        /// <param name="newDueDate">The new due date of the task.</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string UpdateDueDate(string email, string boardName, int taskID, DateTime newDueDate)
        {
            Response r = new Response();
            try
            {

                int boardId = boardController.GetBoardIdByName(email, boardName);
                boardController.GetBoard(boardId).GetTask(taskID).editDueDate(email, taskID,newDueDate,boardId);
                log.Debug("DueDate successfuly update");
               
               
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                r = new Response(e.Message);
            }
            return r.ToJson();
        }

        /// <summary>
        /// This method update the title of the task.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in.</param>
        /// <param name="boardName">The name of the board of the task.</param>
        /// <param name="taskID">The number of the task.</param>
        /// <param name="newTitle">The new title task.</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string UpdadeTitle(string email, string boardName, int taskID, string newTitle)
        {
            Response r = new Response();
            try
            {

                int boardId = boardController.GetBoardIdByName(email, boardName);
                boardController.GetBoard(boardId).GetTask(taskID).editTitle(email,taskID, newTitle,boardId);
                log.Debug("Title successfuly update");
               
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
               r = new Response(e.Message);
            }
            return r.ToJson();
        }

        /// <summary>
        /// This method update the description of the task.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in.</param>
        /// <param name="boardName">The name of the board of the task.</param>
        /// <param name="taskID">The number of the task.</param>
        /// <param name="newDescription">The new description of the task.</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string UpdadeDescription(string email, string boardName, int taskID, string newDescription)
        {
            Response r = new Response();
            try
            {
                int boardId = boardController.GetBoardIdByName(email, boardName);
                boardController.GetBoard(boardId).GetTask(taskID).editDescription(email, taskID, newDescription,boardId);
                log.Debug("Description successfuly update");               
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                r = new Response(e.Message);
            }
            return r.ToJson();
        }

        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>     
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AssignTask(string email, string emailAssignee, string boardName, int taskID)
        {
            Response r = new Response();
            try
            {
                users.PerformOperation(email);
                users.isUserExists(emailAssignee);
                if (boardController.GetUserBoards(email).Contains(boardController.GetBoardIdByName(email,boardName)) && boardController.GetUserBoards(emailAssignee).Contains(boardController.GetBoardIdByName(emailAssignee, boardName)))
                {
                    int boardId = boardController.GetBoardIdByName(email, boardName);
                    boardController.GetBoard(boardId).GetTask(taskID).AssignTask(email, emailAssignee,boardId);
                    log.Debug("Assigned task successfuly update");
                }                 
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                r = new Response(e.Message);
            }
            return r.ToJson();
        }

    }
}
