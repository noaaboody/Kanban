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
    /// A class of columns DTO. 
    /// Each of the class' methods present SQL query.
    /// </summary
    internal class ColumnDTOMapper : DALcontroller
    {
        private const string TableName = "Column";

        public ColumnDTOMapper() : base(TableName)
        {
        }

        /// <summary>
        /// converts SQLiteDataReader into a ColumnDTO
        /// </summary>
        /// <param name="reader">reader that represents a Column in "Column" table</param>
        /// <returns>new ColumnDTO object</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            ColumnDTO result = new ColumnDTO((int)Convert.ToInt64(reader.GetValue(0)), (int)Convert.ToInt64(reader.GetValue(1)), (int)Convert.ToInt64(reader.GetValue(2)));
            return result;
        }

        /// <summary>
        /// creates a list containing all Columns DTO objects in "Column" table
        /// </summary>
        /// <returns>list of all Columns DTO in "Column" table</returns>
        public List<ColumnDTO> SelectAllColumns()
        {
            List<ColumnDTO> result = Select().Cast<ColumnDTO>().ToList();
            return result;
        }

        /// <summary>
        /// inserts a new Column to "Column" table
        /// </summary>
        /// <param name="column">ColumnDTO represents a new Column to be inserted</param>
        /// <returns> true if inserted correctly, false elsewise</returns>
        /// <exception cref="Exception">throws a proper Exception according to the SQL commands</exception>
        public bool Insert(ColumnDTO column)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({ColumnDTO.ColumnColumnOrdColumnName} ,{ColumnDTO.ColumnBoardIDColumnName},{ColumnDTO.ColumnMaxLengthColumnName}) " +
                    $"VALUES (@ColumnOrdVal,@BoardIDVal, @MaxLengthVal); ";

                    SQLiteParameter ColumnOrdParam = new SQLiteParameter(@"ColumnOrdVal", column.ColumnOrd);
                    SQLiteParameter BoardIDParam = new SQLiteParameter(@"BoardIDVal", column.BoardID);
                    SQLiteParameter MaxLengthParam = new SQLiteParameter(@"MaxLengthVal", column.MaxLength);


                    command.Parameters.Add(ColumnOrdParam);
                    command.Parameters.Add(BoardIDParam);
                    command.Parameters.Add(MaxLengthParam);

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
        /// Deletes a Column from "Column" table
        /// </summary>
        /// <param name="columnDTO">Column's DTO to be deleted</param>
        /// <returns>true if deleted succesfully , false elsewise</returns>
        public bool Delete(ColumnDTO columnDTO)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where ColumnOrd={columnDTO.ColumnOrd} and BoardID={columnDTO.BoardID}"
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

