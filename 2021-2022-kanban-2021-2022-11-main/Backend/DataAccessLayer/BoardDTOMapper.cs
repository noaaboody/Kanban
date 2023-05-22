using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /// <summary>
    /// A class of boards DTO. 
    /// Each of the class' methods present SQL query.
    /// </summary
    internal class BoardDTOMapper : DALcontroller
    {
        private const string TableName = "Board";

        public BoardDTOMapper() : base(TableName)
        {
        }

        /// <summary>
        /// converts SQLiteDataReader into a BoardDTO
        /// </summary>
        /// <param name="reader">reader that represents a Board in "Board" table</param>
        /// <returns>new BoardDTO object</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardDTO result = new BoardDTO((int)Convert.ToInt64(reader.GetValue(0)), reader.GetString(1), reader.GetString(2), (int)Convert.ToInt64(reader.GetValue(3)));
            return result;
        }

        /// <summary>
        /// creates a list containing all Board DTO objects in "Board" table
        /// </summary>
        /// <returns>list of all Boards DTO in "Board" table</returns>
        public List<BoardDTO> SelectAllBoards()
        {
            List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();
            return result;
        }


        /// <summary>
        /// inserts a new Board to "Board" table
        /// </summary>
        /// <param name="board">BoardDTO represents a new Board to be inserted</param>
        /// <returns> true if inserted correctly, false elsewise</returns>
        /// <exception cref="Exception">throws a proper Exception according to the SQL commands</exception>
        public bool Insert(BoardDTO board)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({BoardDTO.BoardIDColumnName} ,{BoardDTO.BoardNameColumnName},{BoardDTO.BoardOwnerColumnName},{BoardDTO.BoardCurrentTaskIDColumnName}) " +
                    $"VALUES (@IDVal,@BoardNameVal,@OwnerVal,@CurrentTaskIDVal);";

                    SQLiteParameter IDParam = new SQLiteParameter(@"IDVal", board.BoardID);
                    SQLiteParameter BoardNameParam = new SQLiteParameter(@"BoardNameVal", board.BoardName);
                    SQLiteParameter OwnerParam = new SQLiteParameter(@"OwnerVal", board.BoardOwner);
                    SQLiteParameter CurrentTaskIDParam = new SQLiteParameter(@"CurrentTaskIDVal", board.CurrentTaskID);


                    command.Parameters.Add(IDParam);
                    command.Parameters.Add(BoardNameParam);
                    command.Parameters.Add(OwnerParam);
                    command.Parameters.Add(CurrentTaskIDParam);

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //how to implement that part
                    throw new Exception(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }
                return res > 0;

            }
        }

        /// <summary>
        /// Deletes a Board from "Board" table
        /// </summary>
        /// <param name="boardDTO">Board's DTO to be deleted</param>
        /// <returns>true if deleted succesfully , false elsewise</returns>
        public bool Delete(BoardDTO boardDTO)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where ID={boardDTO.BoardID}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
    }
}

