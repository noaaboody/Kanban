using IntroSE.Kanban.Backend.BuisnessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{

    /// <summary>
    /// A class of boards DTO. 
    /// </summary

    internal class BoardDTO : DTO
    {

        public const string BoardIDColumnName = "ID";
        public const string BoardNameColumnName = "Name";
        public const string BoardOwnerColumnName = "Owner";
        public const string BoardCurrentTaskIDColumnName = "CurrentTaskID";


        private int _BoardID;
        private string _BoardName;
        private string _BoardOwner;
        private int _CurrentTaskID;

        public int BoardID { get => _BoardID; set { _BoardID = value; _controller.Update(BoardID, BoardIDColumnName , value); } }
        public string BoardName { get => _BoardName; set { _BoardName = value; _controller.Update(BoardID, BoardNameColumnName, value); } }
        public string BoardOwner { get => _BoardOwner; set { _BoardOwner = value; _controller.Update(BoardID, BoardOwnerColumnName, value); } }
        public int CurrentTaskID { get => _CurrentTaskID; set { _CurrentTaskID = value; _controller.Update(BoardID, BoardCurrentTaskIDColumnName, value); } }
        

        public BoardDTO(int BoardID, string BoardName, string BoardOwner, int CurrentTaskID) : base(new BoardDTOMapper())
        {
            _BoardID= BoardID;
            _BoardName = BoardName;
            _BoardOwner = BoardOwner;
            _CurrentTaskID = CurrentTaskID;
        }


    }
}
