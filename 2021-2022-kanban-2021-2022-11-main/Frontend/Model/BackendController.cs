using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Text.Json;
using IntroSE.Kanban.Backend.BuisnessLayer;

namespace Frontend.Model
{
    /// <summary>
    /// represent the connection between the frontend to the backend  
    /// </summary>
    public class BackendController
    {
        private FactoryService factory { get; set; }
        public BackendController(FactoryService factory)
        {
            this.factory = factory;
        }

        public BackendController()
        {
            this.factory = new FactoryService();
            factory.LoadData(); 
            
                      
        }


        /// <summary>
        /// This method loggs In the user.
        /// </summary>
        /// <param name="username">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>UserModel, unless an error occurs</returns>
        public UserModel Login(string username, string password)
        {
            String user = factory.userService.LogIn(username, password);          
            Dictionary<string, string>? login = JsonSerializer.Deserialize<Dictionary<string, string>>(user);
            if (login.ContainsKey("ErrorMessage"))
            {
                throw new Exception(login["ErrorMessage"]);
            }
            return new UserModel(this, username);
        }

        /// <summary>
        /// This method registers a new user.
        /// </summary>
        /// <param name="username">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>UserModel, unless an error occurs</returns>
        internal UserModel Register(string username, string password)
        {
            String register = factory.userService.Register(username, password);
            Dictionary<string, string>? reg = JsonSerializer.Deserialize<Dictionary<string, string>>(register);
            if (reg.ContainsKey("ErrorMessage"))
            {
                throw new Exception(reg["ErrorMessage"]);
            }
            return new UserModel(this, username);
        }


        /// <summary>
        /// This method returns a board's ID and name using its ID.
        /// </summary>
        /// <param name="boardId">The email of the user.</param>
        /// <returns>int id and string title, unless an error occurs</returns>
        internal (int Id, string title) GetBoardsName(int boardId)
        {
            string ans=factory.boardService.GetBoardName(boardId);
            Dictionary<string , string>? board = JsonSerializer.Deserialize<Dictionary<string , string>>(ans);
            if (board.ContainsKey("ErrorMessage"))
            {
                throw new Exception(board["ErrorMessage"]);
            }
            return (boardId, board["ReturnValue"]);
        }

        /// <summary>
        /// This method returns a list containing all IDs of the boards a user is a member of.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>list of ID board's user</returns>
        public List<int> GetUserBoardIDs(string email)
        {
            return factory.boardService.GetBoardController().GetUserBoards(email);  
        }

        /// <summary>
        /// This method returns all Task deatils of a specified column
        /// </summary>
        /// <param name="email">The email of the user that own the board.</param>
        /// <param name="boardname">The name of the board of the task</param>
        /// <param name="columnord">Column ordinal (counting from 0 to 2)</param>
        /// <returns>list of the details, unless an error occurs</returns>
        public List<TaskModel.TaskDetailsModel> getTasksDetails(string email, string boardname, int columnord)
        {
            string ans = factory.boardService.GetColumn(email, boardname, columnord);
            Dictionary<string, string>? columnJ = JsonSerializer.Deserialize<Dictionary<string, string>>(ans);
            if (columnJ.ContainsKey("ErrorMessage"))
            {
                throw new Exception(columnJ["ErrorMessage"].ToString());
            }

            String s = columnJ["ReturnValue"];
            List<TaskModel.TaskDetailsModel> tasks = JsonSerializer.Deserialize<List<TaskModel.TaskDetailsModel>>(s);
            return tasks;
        }

        
    }
}
