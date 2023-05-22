using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestProject")]
namespace IntroSE.Kanban.Backend.ServiceLayer
{

        /// <summary>
        /// A class of the user actions on the system. 
        /// <para>
        /// Each of the class' methods should return a string.
        /// </para>
        /// </summary>
    
    public class UserService
    {
        private UserController userController;
        private BoardController boardController;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserService(UserController control, BoardController boardController)
        {
            this.userController = control;
            this.boardController = boardController;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log!");
        }

        public UserController getUserController() {return userController; }
        public BoardController GetBoardController() { return boardController; }

        /// <summary>
        /// This method loggs In the user.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>with user email, unless an error occurs</returns>
        public string LogIn(string email, string password)
        {
            Response r = new Response();
            log.Debug($"logging in user {email}");
            try
            {
                userController.LogIn(email, password);
                r.ReturnValue = email;
                log.Info("user logged in succesfully");               
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                r.ErrorMessage = (e.Message);            
            }
            return r.ToJson();
        }

        /// <summary>
        /// This method loggs out the user.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string LogOut(string email)
        {
            Response r = new Response();
            log.Debug($"logging out user {email}");
            try
            {
                userController.LogOut(email);
                log.Info($"user {email} logget out succesfully");
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                r.ErrorMessage = (e.Message);                
            }
            return r.ToJson();
        }

        /// <summary>
        /// This method creates a new user.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string Register(string email, string password)
        {
            Response r = new Response();
            log.Debug("registration of a user");
            try
            {
                userController.Register(email, password);                
                log.Info("user registered succesfuly");                
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                r.ErrorMessage = e.Message;
            }
           return r.ToJson();
        }

        /// <summary>
        /// This method changes the password of an existing user.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="newPassword">The password of the user.</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string ChangePassword(string email ,string newPassword)
        {
            Response r = new Response();
            log.Debug($"changing user {email} password to {newPassword}");
            try
            {
                userController.ChangePassword(email, newPassword);
                r.ReturnValue = (email);
                log.Info("password changed succesfully");               
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                r.ErrorMessage = (e.Message);             
            }
            return r.ToJson();
        }


        /// <summary>
        /// This methos joins a user into an existing board
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardID"></param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string JoinBoard(string email, int boardID)
        {
            Response r =new Response();
            log.Debug($"adding user {email} to board {email}");
            try
            {
                boardController.JoinBoard(email, boardID);
                log.Info($"user {email} was added to board{boardID} succesfully");               
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                r.ErrorMessage = (e.Message);
            }
            return r.ToJson();
        }

        /// <summary>
        /// this method removes an existing user from a related board
        /// </summary>
        /// <param name="email">user email to be removed</param>
        /// <param name="BoardTitle">board title to extract user from</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string LeaveBoard(string email, string BoardTitle)
        {
            Response r = new Response();
            log.Debug($"removing board {BoardTitle} from user {email}");
            try
            {
                boardController.RemoveUserFromBoard(email, BoardTitle);              
                log.Info($"board {BoardTitle} removed succesfully");             
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                r.ErrorMessage = (e.Message);
            }
            return r.ToJson();
        }

        /// <summary>
        /// Returns all InProgress Tasks an existing user has in the system.
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>User's InProgress Task List, unless an error occurs</returns>
        public string GetInProgress(string email)
        {
            Response r = new Response();
            log.Debug($"Getting 'in progress' tasks for user {email}");
            try
            {
                List<BuisnessLayer.Task> taskList = boardController.GetInProgress(email);  
                r.ReturnValue = taskList;
                log.Info("In progress tasks pulled succesfully");              
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                r.ErrorMessage = (e.Message);
            }
            return r.ToJson();
        }


    }
}
