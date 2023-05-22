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
    /// A class of task DTO. 
    /// Each of the class' methods present SQL query.
    /// </summary
    internal class TaskDTOMapper : DALcontroller
    {
        private const string TaskTableName = "Task";

        public TaskDTOMapper() : base(TaskTableName)
        {
        }

        /// <summary>
        /// creates a list containing all Tasks DTO objects in "Task" table
        /// </summary>
        /// <returns>list of all Tasks DTO in "Task" table</returns>
        public List<TaskDTO> SelectAllTasks()
        {
            List<TaskDTO> result = Select().Cast<TaskDTO>().ToList();

            return result;
        }

        /// <summary>
        /// converts SQLiteDataReader into a TaskDTO
        /// </summary>
        /// <param name="reader">reader that represents a Task in "Task" table</param>
        /// <returns>new TaskDTO object</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            TaskDTO result = new TaskDTO((int)Convert.ToInt64(reader.GetValue(0)), (int)Convert.ToInt64(reader.GetValue(1)), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), (int)Convert.ToInt64(reader.GetValue(7)), (int)Convert.ToInt64(reader.GetValue(8)));
            return result;
        }

        /// <summary>
        /// inserts a new Task to "Task" table
        /// </summary>
        /// <param name="task">TaskDTO represents a new Task to be inserted</param>
        /// <returns> true if inserted correctly, false elsewise</returns>
        /// <exception cref="Exception">throws a proper Exception according to the SQL commands</exception>
        public bool Insert(TaskDTO task)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} ({TaskDTO.TaskIDColumnName} ,{TaskDTO.TaskColumnOrdColumnName},{TaskDTO.TaskBoardIDColumnName},{TaskDTO.TaskTitleColumnName},{TaskDTO.TaskCreationTimeColumnName}, {TaskDTO.TaskDueDateColumnName}, {TaskDTO.TaskDescriptionColumnName}, {TaskDTO.TaskAssigneeColumnName}, {TaskDTO.TaskStateColumnName}) " +
                        $"VALUES (@IDVal,@ColumnOrdVal,@BoardIDVal,@titleVal,@CreationTimeVal,@DueDateVal,@DescriptionVal,@AssigneeVal,@StateVal);";

                    SQLiteParameter IDParam = new SQLiteParameter(@"IDVal", task.ID);
                    SQLiteParameter ColumnOrdParam = new SQLiteParameter(@"ColumnOrdVal", task.ColumnOrd);
                    SQLiteParameter BoardIDParam = new SQLiteParameter(@"BoardIDVal", task.BoardID);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", task.Title);
                    SQLiteParameter CreationTimeParam = new SQLiteParameter(@"CreationTimeVal", task.CreationTime);
                    SQLiteParameter DueDateParam = new SQLiteParameter(@"DueDateVal", task.DueDate);
                    SQLiteParameter DescriptionParam = new SQLiteParameter(@"DescriptionVal", task.Description);
                    SQLiteParameter AssigneeParam = new SQLiteParameter(@"AssigneeVal", task.Assignee);
                    SQLiteParameter StateParam = new SQLiteParameter(@"StateVal", task.State);


                    command.Parameters.Add(IDParam);
                    command.Parameters.Add(ColumnOrdParam);
                    command.Parameters.Add(BoardIDParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(CreationTimeParam);
                    command.Parameters.Add(DueDateParam);
                    command.Parameters.Add(DescriptionParam);
                    command.Parameters.Add(AssigneeParam);
                    command.Parameters.Add(StateParam);

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                    log.Error(ex.Message);
                }
                finally
                {
                    log.Info($"user {task.Title} was insert succesfully");
                    command.Dispose();
                    connection.Close();

                }
                return res > 0;
            }
        }

        /// <summary>
        /// Deletes a Task from "Task" from table
        /// </summary>
        /// <param name="task">Task's DTO to be deleted</param>
        /// <returns>true if deleted succesfully , false elsewise</returns>
        public bool Delete(TaskDTO task)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} WHERE ID= {task.ID} AND BoardID={task.BoardID}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"task {task.ID} was deleted succesfully");
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
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
    }
}

