using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;
using System.Threading.Tasks;
using System.Text.Json;
using System.Runtime.CompilerServices;
using IntroSE.Kanban.Backend.BuisnessLayer;

/*[assembly: InternalsVisibleTo("TestProject")]
*/
namespace IntroSE.Kanban.Backend.ServiceLayer
{

    /// <summary>
    /// A class of the boards actions in the system. 
    /// <para>
    /// Each of the class' methods should return a string.
    /// </para>
    /// </summary>

    public class BoardService//there are several marked functions that are for grading service only ,
    {// the right implementation is a function beneath the grading service only data(our system is built by our architacture and not by grrading service)
        private BoardController boardController { get; set; }
        BuisnessLayer.UserController userController { get; set; }
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>        /// </summary>
        /// <returns>the Board controller</returns>
        public BoardController GetBoardController()
        {
            return boardController;
        }
        /// <summary>
        /// This method is the class Constructor
        /// </summary>
        /// <param name="boardName">The name of the new board</param>
        public BoardService(BuisnessLayer.UserController U, BuisnessLayer.BoardController B)
        {
            userController = U;
            boardController = B;
        }

        /// <summary>
        /// this method is for creating a new board for a user
        /// </summary>
        /// <param name="boardName">The name of the new board</param>
        /// <param name="email">The name of the board</param>

        public string CreateBoard(string email, string boardName)
        {
            log.Debug("Started creating board");

            Response r = new Response();
            try
            {
                boardController.CreateBoard(email, boardName);

            }
            catch (Exception e)
            {
                log.Info("creating board Faild");
                r.ErrorMessage = e.Message;
            }
            return r.ToJson();
        }

        /// <summary>
        /// This method advanced the task to the next board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in.</param>
        /// <param name="TaskId">The number of the task.</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string AdvanceTask(string email, string boardName, int taskId)
        {
            Response r = new Response();
            try
            {
                log.Info("started advancing tasks");
                boardController.AdvanceTask(email, boardName, taskId);
            }
            catch (Exception e)
            {

                r.ErrorMessage = e.Message;
            }
            return r.ToJson();
        }

        /// <summary>
        /// This method set the limit of the tasks in the column.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in.</param>
        /// <param name="boardTitle">The title of the board.</param>
        /// <param name="columnOrd">The number of the column.</param>
        /// <param name="limit">The number limit of the tasks.</param>
        /// <returns>An empty response, unless an error occurs.</returns>

        public string SetColumnLimit(string email, string BoardName, int columnOrd, int limit)
        {
            Response r = new Response();
            try
            {

                boardController.SetColumnLimit(email, BoardName, columnOrd, limit);
            }
            catch (Exception e)
            {
                r.ErrorMessage = e.Message;
            }
            return r.ToJson();
        }
        public string SetColumnLimit(string email, int BoardID, int columnOrd, int limit)
        {
            Response r = new Response();
            try
            {
                boardController.SetColumnLimit(email, BoardID, columnOrd, limit);
            }

            catch (Exception e)
            {
                r.ErrorMessage = e.Message;
            }
            return r.ToJson();
        }


        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetUserBoards(string email)
        {
            Response r = new Response();
            try
            {
                r.ReturnValue = boardController.GetUserBoards(email);
            }
            catch (Exception e)
            {
                r.ErrorMessage = e.Message;
            }
            return r.ToJson();
        }


        /// <summary>
        /// This method returns a column given it's name 
        ///  <para><b>IMPORTANT: task in the rerurned column should be deserialized again</b></para>
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>       
        /// <returns>A response with a list of the column's tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {
            Response r = new Response();
            try
            {
                r.ReturnValue = JsonSerializer.Serialize(boardController.GetColumn(email, boardName, columnOrdinal));
            }
            catch (Exception e)
            {
                r.ErrorMessage = e.Message;
            }
            return r.ToJson();
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            Response r = new Response();
            try
            {
                int id = boardController.GetBoardIdByName(email, boardName);
                r.ReturnValue = (boardController.GetBoard(id).GetColumnName(columnOrdinal));
            }
            catch (Exception e)
            {
                r.ErrorMessage = e.Message;
            }
            return r.ToJson();
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's limit, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)//only for grading service Only 
        {
            Response r = new Response();
            try
            {
                int id = boardController.GetBoardIdByName(email, boardName);
                r.ReturnValue = boardController.GetBoard(id).GetColumnLimit(columnOrdinal);
            }
            catch (Exception e)
            {
                r.ErrorMessage = e.Message;
            }
            return r.ToJson();
        }


        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetBoardName(int boardId)
        {
            Response r = new Response();
            try
            {
                r.ReturnValue = boardController.getBoardName(boardId);

            }
            catch (Exception e)
            {
                r.ErrorMessage = e.Message;
            }
            return r.ToJson();
        }


        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>

        public string RemoveUser(string email, int BoardId)
        {
            Response r = new Response();
            try
            {
                boardController.RemoveUserFromBoard(email, BoardId);
            }
            catch (Exception e)
            {
                r.ErrorMessage = e.Message;
            }
            return r.ToJson();
        }

        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string RemoveBoard(string email, string name)
        {
            Response r = new Response();
            log.Debug($"Removing current Board" + name);
            try
            {
                int id = boardController.GetBoardIdByName(email, name);
                boardController.RemoveBoard(email, name);
                log.Info("Board Removed");

            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                r.ErrorMessage = e.Message;
            }
            return r.ToJson();


        }
        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)//for grading service purpuses only
        {
            Response r = new Response();
            log.Debug($"transferring ownerwhip from{currentOwnerEmail} to {newOwnerEmail}");
            try
            {

                boardController.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
                log.Info("ownerwhip transferred");

            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                r.ErrorMessage = e.Message;
            }
            return r.ToJson();
        }



        ///<summary>This method loads all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method. 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LoadData()
        {
            Response r = new Response();
            log.Debug("starting Bard Data loading");
            try
            {
                boardController.LoadData();
                log.Info("Data loaded succesfully");
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                r.ErrorMessage = e.Message;
            }
            return r.ToJson();
        }

    }
}
