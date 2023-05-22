using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    /// <summary>
    /// A class of board DTO. 
    /// </summary
    internal class BoardUserDTO : DTO
    {
        public const string BoardUserBoardIDColumnName = "BoardID";
        public const string BoardUserEmailColumnName = "Email";


        private int _BoardID;
        private string _Email;
        

        public int BoardID { get => _BoardID; set { _BoardID = value; _controller.Update(BoardID, Email, BoardUserBoardIDColumnName, value); } }
        public string Email { get => _Email; set { _Email= value; _controller.Update(BoardID, Email, BoardUserEmailColumnName, value); } }



        public BoardUserDTO(int BoardID, string Email): base(new BoardDTOMapper())
        {
            _BoardID = BoardID;
            _Email = Email;
        }


    }
}
