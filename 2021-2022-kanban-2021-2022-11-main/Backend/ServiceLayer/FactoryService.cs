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

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    /// A class of the controller and the service layer classes.
    /// </summary>
    public class FactoryService
    {
        private UserController userController;
        private BoardController boardController;
        public UserService userService;
        public BoardService boardService;
        public TaskService taskService;


      public FactoryService()
        {
            boardController = new BoardController();
            userController = boardController.GetUserController();
            userService = new UserService(userController,boardController);
            boardService = new BoardService(userController, boardController);
            taskService = new TaskService(userController,boardController);
        }

        public string LoadData()
        {
            Response r = new Response();
            try
            {
                userController.LoadAllUsers();
                boardController.LoadData();
            }
            catch (Exception ex)
            {
                r.ErrorMessage= ex.Message;
            }
            return r.ToJson();
        }

        public string DeleteAllData()
        {
            Response r = new Response();
            try
            {
                userController.DeleteAllUsers();
                boardController.DeleteAllData();
            }
            catch(Exception ex)
            {
                r.ErrorMessage= ex.Message;
            }
            return r.ToJson();
        }
    }
}
