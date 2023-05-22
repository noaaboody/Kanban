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
    /// A class of board and user DTO. 
    /// Each of the class' methods present SQL query.
    /// </summary
    internal class BoardUserDTOMapper : DALcontroller
    {
        private const string TableName = "BoardUser";

        public BoardUserDTOMapper() : base(TableName)
        {
        }

        /// <summary>
        /// converts SQLiteDataReader into a BoardUserDTO
        /// </summary>
        /// <param name="reader">reader that represents a BoardUser in "BoardUser" table</param>
        /// <returns>new BoardUserDTO object</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardUserDTO result = new BoardUserDTO((int)Convert.ToInt64(reader.GetValue(0)), reader.GetString(1));
            return result;
        }

        /// <summary>
        /// creates a list containing all BoardUsers DTO objects in "BoardUser" table
        /// </summary>
        /// <returns>list of all BoardUsers DTO in "BoardUser" table</returns>
        public List<BoardUserDTO> SelectAllBoardUsers()
        {
            List<BoardUserDTO> result = Select().Cast<BoardUserDTO>().ToList();
            return result;
        }

        /// <summary>
        /// inserts a new BoardUser to "BoardUser" table
        /// </summary>
        /// <param name="boardUser">BoardUserDTO represents a new BoardUser to be inserted</param>
        /// <returns> true if inserted correctly, false elsewise</returns>
        /// <exception cref="Exception">throws a proper Exception according to the SQL commands</exception>
        public bool Insert(BoardUserDTO boardUser)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({BoardUserDTO.BoardUserBoardIDColumnName} ,{BoardUserDTO.BoardUserEmailColumnName}) " +
                    $"VALUES (@BoardIDVal,@EmailVal);";

                    SQLiteParameter BoardIDParam = new SQLiteParameter(@"BoardIDVal", boardUser.BoardID);
                    SQLiteParameter EmailParam = new SQLiteParameter(@"EmailVal", boardUser.Email);


                    command.Parameters.Add(BoardIDParam);
                    command.Parameters.Add(EmailParam);

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
        /// Deletes a BoardUser from "BoardUser" table
        /// </summary>
        /// <param name="boardUserDTO">BoardUser's DTO to be deleted</param>
        /// <returns>true if deleted succesfully , false elsewise</returns>
        public bool Delete(BoardUserDTO boardUserDTO)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} WHERE BoardID= {boardUserDTO.BoardID} AND Email= '{boardUserDTO.Email}' "
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

                return res > 0;
            }
        }
    }
}