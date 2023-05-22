using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;


namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public class Column
    {
        private int maxLength;
        public Dictionary<int, Task> tasks = new Dictionary<int, Task>();
        private int ord;
        private int boardId;

        /// <summary>
        /// a constructor for column Class
        /// </summary>
        public Column(int ord,int BoardId)
        {
            this.ord = ord; 
            this.boardId = BoardId;
            this.maxLength = -1;
            tasks = new Dictionary<int, Task>();
        }

        /// <summary>
        /// This method is used for stting a column maximum amount of tasks
        /// </summary>
        public void SetColumnLimit(int limit)
        {
            this.maxLength = limit;
        }
        /// <summary>
        /// This method is used for removing all of column tasks for DB
        /// </summary>
        public void ReMoveColumn()
        {
            foreach (Task task in tasks.Values)
            {
                task.DeleteTask();
            }
        }

     

        /// <summary>
        /// This method return the maximum amount of takss a column can have
        /// </summary>
        /// <returns> the maximum amount of takss a column can have</returns>
        public int getMaxLength()
            { return this.maxLength; }
        

    }
}
