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
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;


namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public class BoardController
    {
        public Dictionary<int, Board> boards = new Dictionary<int, Board>();
        public UserController users = new UserController();
        TaskDTOMapper taskMapper= new TaskDTOMapper();
        public int CurrBoardId = 0;
        private BoardDTOMapper boardMapper= new BoardDTOMapper();
        private BoardUserDTOMapper boardUserMapper = new BoardUserDTOMapper();
        private ColumnDTOMapper ColumnMapper = new ColumnDTOMapper();


        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);




        //public BoardControllerDTO boardDTOController; ****************
       
        public UserController GetUserController()
        {
            return users;   
        }

        // <summary>
        /// method used for trimming the boardname and cheking if it is legal
        /// </summary>
        /// <param name="boardName"> the name of the board</param>
        /// <returns>the string after trim' if it is illegal it throws an exception</returns>
        /// <exception cref="Exception"></exception>        
         public string ReturnLegalBoardName(string boardname)
        { 
            if(boardname == null)
            {
                log.Error("boardName is null");
                throw new Exception("cannot accept null values");
            }

            boardname = boardname.Trim();
            if(boardname.Length == 0)
            {
                log.Error("boardName is an empty string");
                throw new Exception("cannot accept empty strings as names");
            }

            return boardname;
        }
        /// <summary>
        /// method used for adding a new board to an existing User in the system, user must be logged in.
        /// board name can not be empty and a user can not have two boards with the same name.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// 
       
        public void CreateBoard(string email, string boardName)
        {
            email = email.Trim().ToLower();
            log.Debug($"adding board {boardName}");
            if (email == null)
            {
                log.Error("Email is null");
                throw new Exception("cannot accept null values");
            }
            email = email.ToLower();
            email = email.Trim();
            boardName = ReturnLegalBoardName(boardName);
            users.PerformOperation(email);
            if (DoesUserHasBoard(email, boardName))
            {
                log.Error($"board {boardName} already exists for {email}");
                throw new Exception($"board {boardName} already exists for {email}");
            }
            Board B= new Board(boardName, email, CurrBoardId);
            BoardUserDTO dto = new BoardUserDTO(B.boardID, email);
            boardUserMapper.Insert(dto);
            BoardDTO dt = new BoardDTO(B.boardID, B.getName(), B.owner, B.GetCurrentTaskId());
            boards.Add(B.boardID,B);
            boardMapper.Insert(dt);
            boards[CurrBoardId] = B;
            ColumnMapper.Insert(new ColumnDTO(0, B.boardID, -1));
            ColumnMapper.Insert(new ColumnDTO(1, B.boardID, -1));
            ColumnMapper.Insert(new ColumnDTO(2, B.boardID, -1));
            CurrBoardId++;
            log.Debug("The board successfully added");
        }

        /// </summary>checks if user has a certain board name </summary>
        /// <param name="boardName">the name of the Board</param>
        /// <param name="email">the email if the user</param
        /// <returns>the true if he has a board and false if not</returns>
        private bool DoesUserHasBoard(string email, string boardName)
        {
            if (email == null)
                throw new Exception("email is null");          
            email = email.ToLower().Trim();
            List<int> boardIds = GetUserBoards(email);
            foreach(int id in  boardIds)
                {
                if (boardName.Equals(GetBoard(id).getName()))
                    {
                    return true; 
                    }
                }

            return false;
        }
        /// </summary>    </summary>
        /// <param name="BoardId">the id of the Board</param>
        /// <param name="boardName"></param>
        /// <returns>the board</returns>
        /// <exception cref="Exception">if the id is not the id of any voard</exception>
        public Board GetBoard(int BoardId)
        {
            if (BoardId > CurrBoardId)
            {
                log.Error($"Failed to get board because boardID - {BoardId} does not exist");
                throw new Exception("ID does not exist");
            }
            return boards[BoardId];
        }

        /// <summary>
        /// method used to add a certain user to a board
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardId"></param>
        /// <returns>void</returns>
        public void JoinBoard(string email, int boardId)
        {
            if (email == null)
                throw new Exception("email is null");
            email = email.Trim().ToLower(); 
            users.PerformOperation(email);
            Board B = GetBoard(boardId);
            B.JoinBoard(email, boardId);
            BoardUserDTO dto = new BoardUserDTO(boardId, email);
            boardUserMapper.Insert(dto);
        }
        /// <summary>
        /// method used to Get all of users in progress Tasks
        /// </summary>
        /// <param name="email"></param>
        /// <returns>a List of Tasks</returns>
        public List<Task> GetInProgress(string email)
        {
            if (email == null)
                throw new Exception("email is null");
            email = email.Trim().ToLower();
            List<Task> list = new List<Task>();
            users.PerformOperation(email);
            foreach (Board board in boards.Values)
            {
                if (board.users.Contains(email))
                {
                    List <Task> l = board.GetInProgress(email);
                    list.AddRange(l);
                }
            }

            return list;
        }
        /// <summary>
        /// This method set the limit of the tasks in the column.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in.</param>
        /// <param name="boardTitle">The title of the board.</param>
        /// <param name="columnOrd">The number of the column.</param>
        /// <param name="limit">The number limit of the tasks.</param>
        /// <returns>An empty response, unless an error occurs.</returns>
        public void SetColumnLimit(string email, string BoardName, int columnOrd, int limit)//this is only for grading service purposes only by achiya it is ok
        {
            if (email == null)
                throw new Exception("email is null");
            email = email.Trim().ToLower();

                users.PerformOperation(email);
                int id = GetBoardIdByName(email, BoardName.Trim());
                GetBoard(id).SetColumnLimit(columnOrd, limit);
            if (!ColumnMapper.Update(columnOrd, id, "MaxLength", limit))
            {
                log.Error("update DB Failed");
                throw new Exception("DB Update failed");
            }
        }
        /// <summary>
        /// This method set the limit of the tasks in the column.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in.</param>
        /// <param name="Boardid">The title of the board.</param>
        /// <param name="columnOrd">The number of the column.</param>
        /// <param name="limit">The number limit of the tasks.</param>
        /// <returns>An empty response, unless an error occurs.</returns>
        public void SetColumnLimit(string email, int Boardid, int columnOrd, int limit)//this is only for grading service purposes only by achiya it is ok
        {
            if (email == null)
                throw new Exception("email is null");
            email = email.Trim().ToLower();
            users.PerformOperation(email);
            GetBoard(Boardid).SetColumnLimit(columnOrd, limit);
            if (!ColumnMapper.Update(columnOrd, Boardid, "MaxLength", limit))
            {
                log.Error("update DB Failed");
                throw new Exception("DB Update failed");
            }
        }


        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns> a list of the column's tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public List<Task> GetColumn(string email, string boardName, int columnOrdinal)
        {
            if (email == null)
            { throw new Exception("email is null"); }
            email = email.Trim().ToLower();
            users.PerformOperation(email);
            int id = GetBoardIdByName(email, boardName);
            List<Task> l = GetBoard(id).GetColumn(columnOrdinal);
            return l;
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>the column's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            if (email == null)
                throw new Exception("email is null");
            email = email.Trim().ToLower();
            users.PerformOperation(email);
            string n =GetBoardByName(email, boardName).GetColumnName(columnOrdinal);
            return n;
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>the column's limit, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public int GetColumnLimit(string email, string boardName, int columnOrdinal)//only for grading service Only 
        {
            if (email == null)
                throw new Exception("email is null");
            email = email.Trim().ToLower();
            users.PerformOperation(email);
           int i= GetBoardByName(email,boardName).GetColumnLimit(columnOrdinal);
          return i;
        }

        /// <summary>
        /// method used to get all of the users boards ids
        /// </summary>
        /// <param name="email"></param>
        /// <returns>a list of int</returns>
        public List <int> GetUserBoards(string email)
        {
            if (email == null)
                throw new Exception("email is null");
            email = email.Trim().ToLower();
            users.PerformOperation(email);
            List<int> UserBoards = new List<int>();
            foreach (Board B in boards.Values)
            {
                if (B.GetUsersOfBoard().Contains(email))
                {
                    UserBoards.Add(B.boardID);
                }
            }
            return UserBoards;
       
        }

        /// <summary>
        /// method used to get a board id with email of user using the board and the boards name
        /// </summary>
        /// <param name="email">the users email</param>
        /// <param name="name">the boards name</param>
        /// <returns>int representing the board id</returns>
        public int GetBoardIdByName(string email, string name)
        {
            users.PerformOperation(email);
            email = email.Trim().ToLower();
            int boardId = -1;
            foreach (Board B in boards.Values)
            {
                if (B.getName().Equals(name) && B.GetUsersOfBoard().Contains(email))
                {
                    boardId = B.boardID;
                }
            }

            if (boardId == -1)
            {
                log.Error($"Failed to get board because email and name of board does not match ");
                throw new Exception("name of board and email of user does not match");
            }

            return boardId;

        }


        /// <summary>
        /// method used to get a board id with email of user using the board and the boards name
        /// </summary>
        /// <param name="email">the users email</param>
        /// <param name="name">the boards name</param>
        /// <returns>int representing the board id</returns>
        public Board GetBoardByName(string email, string name)//function for grading service only
        {
            if (email == null)
                throw new Exception("email is null");
            users.PerformOperation(email);
            email = email.Trim().ToLower();
            int boardId = -1;
            foreach (Board B in boards.Values)
            {
                if (B.getName().Equals(name) && B.GetUsersOfBoard().Contains(email))
                {
                    boardId = B.boardID;
                }
            }

            if (boardId == -1)
            {
                log.Error($"Failed to get board because email and name of board does not match ");
                throw new Exception("name of board and email of user does not match");
            }

            return GetBoard(boardId);
        }

        /// <summary>
        /// method used to check if a user is already in board using boards id and users email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardId"></param>
        /// <returns>void</returns>
        public void IsUserAlreadyInBoard(string email, int boardId)//function for grading service only
        {
            if (email == null)
                throw new Exception("email is null");

            email = email.Trim().ToLower();
            if (this.GetBoard(boardId).users.Contains(email))
            {
                log.Error($"Failedadd user to board becouse user has board by this name ");
                throw new Exception("name of board already in users boards");
            }
        }

        /// <summary>
        /// Removes a user from board
        /// </summary>
        /// <param name="email">the user whoes being removed</param>
        /// <param name="id">ghe boards nid</param>
        /// <returns>void</returns>
        public void RemoveUserFromBoard(string email, int id)
        {
            log.Debug("removing board");
            if (email == null)
            {
                log.Error($"email is Null");
                throw new Exception("cannot accept null values");
            }
            email = email.ToLower().Trim();
            users.PerformOperation(email);
           
            Board board = GetBoard(id);
            board.RemoveUser(email,id);
            boardUserMapper.Delete(new BoardUserDTO(board.boardID, email));
            log.Debug($"board {board.getName()} deleted successfully from User {email}");
        }

        /// <summary>
        /// Removes a user from board
        /// </summary>
        /// <param name="email">the user whoes being removed</param>
        /// <param name="boardName">ghe boards name</param>
        /// <returns>void</returns>
        public void RemoveUserFromBoard(string email, string boardName)
        {
            log.Debug("removing board");
            if (email == null)
            {
                log.Error($"email is Null");
                throw new Exception("cannot accept null values");
            }
            email = email.ToLower().Trim();
            users.PerformOperation(email);
            int id= GetBoardIdByName(email, boardName);
            Board board = GetBoard(id);
            board.RemoveUser(email, id);
            boardUserMapper.Delete(new BoardUserDTO(board.boardID, email));
            log.Debug($"board {board.getName()} deleted successfully from User {email}");
        }

        /// <summary>
        /// method used to get a certain board name based in the boards id
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns>a string representing the boards id</returns>
        public string getBoardName(int boardId)
        {
            return GetBoard(boardId).getName();
        }

        /// <summary>
        /// method used to add a certain user to a board
        /// </summary>
        /// <param name="OwnerEmail">the current board owner</param>
        /// <param name="newOwnerEmail">the new owener</param>
        /// <param name="boardID">the Boards id</param>
        /// <returns>void</returns>
        public void TransferOwnership(string OwnerEmail, string newOwnerEmail, string boardName)
        {
            OwnerEmail = OwnerEmail.ToLower();            
            newOwnerEmail = newOwnerEmail.ToLower();
            users.isUserExists(newOwnerEmail);
            users.PerformOperation(OwnerEmail);
            int id =GetBoardIdByName(OwnerEmail, boardName);           
            GetBoard(id).changeOwner(newOwnerEmail, OwnerEmail);
            if (!boardMapper.Update(id, "Owner", newOwnerEmail))
            {
                log.Error("update DB Failed");
                throw new Exception("DB Update failed");
            }
            
        }


        /// <summary>
        /// method used to advance a task state in a board
        /// </summary>
        /// <param name="email">the email of the user requesting the deletion</param>
        /// <param name="boardName">the Boards name</param>
        /// <param name="taskId">the tasks id</param>
        /// <returns>void</returns>
        public void AdvanceTask(string email, string boardName, int taskId)
        {
            if (email == null)
            {
                log.Info("email is null");
                throw new ArgumentNullException("email");

            }
            email = email.ToLower().Trim();
            users.PerformOperation(email);
            GetBoardByName(email, boardName).AdvanceTask(email, taskId);
            
        }


        /// <summary>
        /// method used to remove a board from the system
        /// </summary>
        /// <param name="email">the email of the user requesting the deletion</param>
        /// <param name="boardName">the Boards name</param>
        /// <returns>void</returns>
        public void RemoveBoard(string email, string boardName)
        {
            users.PerformOperation(email);
            email = email.Trim().ToLower();
            int boardId = GetBoardIdByName(email, boardName);
            Board b = GetBoard(boardId);
            if (b.owner.Equals(email))
            {
         
                List<string> u = b.GetUsersOfBoard();
                foreach (string user in u)
                {
                    boardUserMapper.Delete(new BoardUserDTO(b.boardID, email));
                }
                foreach (Column c in b.columns)
                {
                    c.ReMoveColumn();
                }
                BoardDTO dt= new BoardDTO(b.boardID,b.getName(),b.owner,b.GetCurrentTaskId());
                ColumnMapper.Delete(new ColumnDTO(0, b.boardID, b.GetColumnLimit(0)));
                ColumnMapper.Delete(new ColumnDTO(1, b.boardID, b.GetColumnLimit(1)));
                ColumnMapper.Delete(new ColumnDTO(2, b.boardID, b.GetColumnLimit(2)));
                boardMapper.Delete(dt);
                boards.Remove(boardId);
            }
        }





        ///<summary>This method loads all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method. 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public void LoadData()
        {   
            List<BoardDTO> boardDTOs = boardMapper.SelectAllBoards();
            foreach (BoardDTO boardDTO in boardDTOs)
            {
                Board B = new Board(boardDTO.BoardName, boardDTO.BoardOwner, boardDTO.BoardID);
                boards.Add(B.boardID,B);
                B.LoadData();
            }

            if (boards.Count > 0)
            {
                this.CurrBoardId = boards.Keys.Max() + 1;
            }
        }

        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>

        public void DeleteAllData()
        {
            boards.Clear();
            CurrBoardId = 0;
            boardMapper.DeleteAllData();
            ColumnMapper.DeleteAllData();
            boardUserMapper.DeleteAllData();
            taskMapper.DeleteAllData();
        }
    }
}
