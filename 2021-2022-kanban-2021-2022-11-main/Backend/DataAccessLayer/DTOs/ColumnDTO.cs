using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    /// <summary>
    /// A class of column DTO. 
    /// </summary
    internal class ColumnDTO : DTO 
    {

        public const string ColumnColumnOrdColumnName = "ColumnOrd";
        public const string ColumnBoardIDColumnName = "BoardID";
        public const string ColumnMaxLengthColumnName = "MaxLength";


        private int _ColumnOrd;
        private int _BoardID;
        private int _MaxLength;
        
        public int ColumnOrd { get => _ColumnOrd; set { _ColumnOrd= value; _controller.Update(ColumnOrd,BoardID, ColumnColumnOrdColumnName, value); } }
        public int BoardID { get => _BoardID; set { _BoardID = value; _controller.Update(ColumnOrd, BoardID, ColumnBoardIDColumnName, value); } }
        public int MaxLength { get => _MaxLength; set { _MaxLength = value; _controller.Update(ColumnOrd, BoardID, ColumnMaxLengthColumnName, value); } }


        public ColumnDTO(int ColumnOrd, int BoardID, int maxLength) : base(new ColumnDTOMapper())
        {
            _ColumnOrd = ColumnOrd;
            _BoardID = BoardID;
            _MaxLength = maxLength;
        }

    }
}
