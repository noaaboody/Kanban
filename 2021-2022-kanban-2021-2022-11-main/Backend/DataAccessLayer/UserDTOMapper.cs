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
    /// A class of user DTO. 
    /// Each of the class' methods present SQL query.
    /// </summary
    internal class UserDTOMapper : DALcontroller
    {
        private const string TableName = "User";
        

        public UserDTOMapper() : base(TableName) { }
       
        /// <summary>
        /// converts SQLiteDataReader into a UserDTO
        /// </summary>
        /// <param name="reader">reader that represents a User in "User" table</param>
        /// <returns>new UserDTO object</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserDTO result = new UserDTO(reader.GetString(0), (string)reader.GetString(1));
            return result;
        }
        /// <summary>
        /// creates a list containing all Users DTO objects in "User" table
        /// </summary>
        /// <returns>list of all Users DTO in "User" table</returns>
        public List<UserDTO> SelectAllUsers()
            {
                List<UserDTO> result = Select().Cast<UserDTO>().ToList();
                return result;
            }

        /// <summary>
        /// inserts a new User to "User" table
        /// </summary>
        /// <param name="user">UserDTO represents a new user to be inserted</param>
        /// <returns> true if inserted correctly, false elsewise</returns>
        /// <exception cref="Exception">throws a proper Exception according to the SQL commands</exception>
        public bool Insert(UserDTO user)
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    int res = -1;
                    try
                    {
                        connection.Open();
                        command.CommandText = $"INSERT INTO {TableName} ({UserDTO.UserEmail} ,{UserDTO.UserPassword}) " +
                        $"VALUES (@EmailVal,@PasswordVal);";

                        SQLiteParameter EmailParam = new SQLiteParameter(@"EmailVal", user.Email);
                        SQLiteParameter PasswordParam = new SQLiteParameter(@"PasswordVal", user.Password);
                        
                        command.Parameters.Add(EmailParam);
                        command.Parameters.Add(PasswordParam);
                       
                        command.Prepare();
                        res = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //how to implement that part                                             
                        log.Error(ex.Message);
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
        /// Deletes a User from "User" table
        /// </summary>
        /// <param name="userDTO">User's DTO to be deleted</param>
        /// <returns>ture if deleted succesfully , false elsewise</returns>
        public bool Delete(UserDTO userDTO)
            {
                int res = -1;

                using (var connection = new SQLiteConnection(_connectionString))
                {
                    var command = new SQLiteCommand
                    {
                        Connection = connection,
                        CommandText = $"DELETE FROM {_tableName} WHERE Email= '{userDTO.Email}'"
                    };
                    try
                    {
                        connection.Open();
                        res = command.ExecuteNonQuery();
                    log.Info($"user {userDTO.Email} was deleted succesfully");
                    }
                    catch(Exception ex)
                    {
                    log.Error(ex.Message); 
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

