using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace KanbanTestProj
{
    public class TaskTests
    {
        FactoryService factory;
        TaskService taskService;
        BoardService boardService;
        UserService userService;


        [Test, Order(0)]
        public void Setup()
        {
            factory = new FactoryService();
            taskService = factory.taskService;
            boardService = factory.boardService;
            userService = factory.userService;
            factory.DeleteAllData();


        }


        //createTask Test

        [Test, Order(1)]
        public void createTask_positive_sucsses()
        {
            //arrange
            userService.Register("noa@gmail.com", "Noaaa1");
            boardService.CreateBoard("noa@gmail.com", "HW");
            //act
            taskService.createTask("noa@gmail.com", "HW", "KANBAN", "implement", DateTime.Now.AddDays(1));
            //assert
            Assert.AreEqual(1, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetColumn(0).Count);
        }

        [Test, Order(2)]
        public void createTask_positive2_sucsses()
        {
            //arrange
            //act
            taskService.createTask("noa@gmail.com", "HW", "KANBAN1", "", DateTime.Now.AddDays(1));
            //assert
            Assert.AreEqual(2, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetColumn(0).Count);
        }

        [Test, Order(3)]
        public void createTask_positive3_sucsses()
        {
            //arrange
            //act
            taskService.createTask("noa@gmail.com", "HW", "KANBAN2", null, DateTime.Now.AddDays(1));
            //assert
            Assert.AreEqual(3, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetColumn(0).Count);
        }

        [Test, Order(4)]
        public void createTask_Negative1_sucsses() //the title invalid
        {
            //arrange
            //act
            taskService.createTask("noa@gmail.com", "HW", "  ", "implement", DateTime.Now.AddDays(1));
            //assert
            Assert.AreEqual(3, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetColumn(0).Count);
        }

        [Test, Order(5)]
        public void createTask_Negative2_sucsses() //the title invalid
        {
            //arrange
            //act
            taskService.createTask("noa@gmail.com", "HW", "hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh", "implement", DateTime.Now.AddDays(1));
            //assert
            Assert.AreEqual(3, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetColumn(0).Count);
        }

        [Test, Order(6)]
        public void createTask_Negative3_sucsses() //the description invalid
        {
            //arrange
            //act
            taskService.createTask("noa@gmail.com", "HW", "noaaa", "jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjj", DateTime.Now.AddDays(1));
            //assert
            Assert.AreEqual(3, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetColumn(0).Count);
        }

        [Test, Order(7)]
        public void createTask_Negative4_sucsses() //the duedate invalid
        {
            //arrange
            //act
            taskService.createTask("noa@gmail.com", "HW", "KANBAN", "implement", DateTime.Now.AddDays(-1));
            //assert
            Assert.AreEqual(3, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetColumn(0).Count);
        }

        [Test, Order(8)]
        public void createTask_Negative5_sucsses() //the board name does not exists
        {
            //arrange
            //act
            taskService.createTask("noa@gmail.com", "HWW", "KANBAN", "implement", DateTime.Now.AddDays(1));
            //assert
            Assert.AreEqual(3, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetColumn(0).Count);
        }

        [Test, Order(9)]
        public void createTask_Negative6_sucsses() //the email not exists in the board
        {
            //arrange
            //act
            taskService.createTask("nnoa@gmail.com", "HW", "KANBAN", "implement", DateTime.Now.AddDays(1));
            //assert
            Assert.AreEqual(3, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetColumn(0).Count);
        }

        //createTask with 2 Users
        [Test, Order(10)]
        public void createTask_Positive4_sucsses() //other member add task
        {
            //arrange
            userService.Register("eldori@gmail.com", "Eldi12");
            int boardID = boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW");
            userService.JoinBoard("eldori@gmail.com", boardID);
            //act
            taskService.createTask("eldori@gmail.com", "HW", "KANBANUser33", "implement", DateTime.Now.AddDays(1));
            //assert
            Assert.AreEqual(4, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetColumn(0).Count);
        }

        //no space for more tasks
        [Test, Order(11)]
        public void createTask_Negative7_sucsses() //other member add task
        {
            //arrange
            boardService.CreateBoard("noa@gmail.com", "HW");
            int boardID = boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW");
            userService.JoinBoard("eldori@gmail.com", boardID);
            boardService.SetColumnLimit("noa@gmail.com", "HW", 0, 4);
            //act
            taskService.createTask("eldori@gmail.com", "HW", "KANBANUser12", "implement", DateTime.Now.AddDays(7));
            //assert
            Assert.AreEqual(4, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetColumn(0).Count);
        }





        //AssignTask Tests
        [Test, Order(12)]
        public void AssignTask_Positive1_sucsses()//owner assign task
        {
            //arrange
            //act
            taskService.AssignTask("noa@gmail.com", "eldori@gmail.com", "HW", 0);
            //assert
            Assert.AreEqual(true, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(0).isAssignee("eldori@gmail.com"));
        }

        [Test, Order(13)]
        public void AssignTask_Positive2_sucsses()//not owner assign task
        {
            //arrange
            //act
            taskService.AssignTask("eldori@gmail.com", "eldori@gmail.com", "HW", 3);
            //assert
            Assert.AreEqual(true, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(0).isAssignee("eldori@gmail.com"));
        }

        [Test, Order(14)]
        public void AssignTask_Positive3_sucsses()//assignee change assign task
        {
            //arrange
            //act
            taskService.AssignTask("noa@gmail.com", "eldori@gmail.com", "HW", 0);
            //assert
            Assert.AreEqual(true, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(3).isAssignee("eldori@gmail.com"));
        }


        [Test, Order(16)]
        public void AssignTask_Negative1_sucsses()//not assignee change assigned task assignee
        {
            //arrange
            //act
            taskService.AssignTask("noa@gmail.com", "eldori@gmail.com", "HW", 3);
            //assert
            Assert.AreEqual(true, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(3).isAssignee("eldori@gmail.com"));
        }

        [Test, Order(17)]
        public void AssignTask_Negative2_sucsses()//board member assign not member 
        {
            //arrange
            //act
            taskService.AssignTask("eldori@gmail.com", "omer@gmail.com", "HW", 2);
            //assert
            Assert.AreEqual(false, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(2).isAssigned());
        }

        [Test, Order(19)]
        public void AssignTask_Negative4_sucsses()//boardName illegal
        {
            //arrange
            //act
            taskService.AssignTask("eldori@gmail.com", "noa@gmail.com", "H5W", 2);
            ////assert
            Assert.AreEqual(false, boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(2).isAssigned());
        }




        //Update DueDate tests
        [Test, Order(20)]
        public void UpdateDueDate_Positive1_sucsses()//assignee change duedate 
        {
            //arrange
            //act
            taskService.UpdateDueDate("eldori@gmail.com","HW", 0, DateTime.Now.AddDays(78));
            //assert
            Assert.AreEqual(DateTime.Now.AddDays(78).ToString().Substring(0, 19), boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(0).DueDate.ToString().Substring(0, 19));
        }

        [Test, Order(21)]
        public void UpdateDueDate_Negative1_sucsses()//not assignee change duedate 
        {
            //arrange
            //act
            taskService.UpdateDueDate("noa@gmail.com", "HW", 0, DateTime.Now.AddDays(70));
            //assert
            Assert.AreEqual(DateTime.Now.AddDays(78).ToString().Substring(0, 19), boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(0).DueDate.ToString().Substring(0, 19));
        }

        [Test, Order(22)]
        public void UpdateDueDate_Negative2_sucsses()//assignee change illegal duedate 
        {
            //arrange
            //act
            taskService.UpdateDueDate("eldori@gmail.com", "HW", 0, DateTime.Now.AddDays(-50));
            //assert
            Assert.AreEqual(DateTime.Now.AddDays(78).ToString().Substring(0, 19), boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(0).DueDate.ToString().Substring(0,19));
        }



        //Update Title tests
        [Test, Order(23)]
        public void UpdateTitle_Positive1_sucsses()//assignee change title 
        {
            //arrange
            //act
            taskService.UpdadeTitle("eldori@gmail.com", "HW", 0, "newTitle");
            //assert
            Assert.AreEqual("newTitle", boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(0).Title);
        }

        [Test, Order(24)]
        public void UpdateTitle_Negative1_sucsses()//not assignee change title 
        {
            //arrange
            //act
            taskService.UpdadeTitle("noa@gmail.com", "HW", 0, "newTitle");
            //assert
            Assert.AreEqual("newTitle", boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(0).Title);
        }

        [Test, Order(25)]
        public void UpdateTitle_Negative2_sucsses()//assignee change illegal title 
        {
            //arrange
            //act
            taskService.UpdadeTitle("eldori@gmail.com", "HW", 0, " ");
            //assert
            Assert.AreEqual("newTitle", boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(0).Title);
        }

        [Test, Order(26)]
        public void UpdateTitle_Negative3_sucsses()//assignee change illegal title 
        {
            //arrange
            //act
            taskService.UpdadeTitle("eldori@gmail.com", "HW", 0, null);
            //assert
            Assert.AreEqual("newTitle", boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(0).Title);
        }


        //Update Description tests
        [Test, Order(27)]
        public void UpdateDescription_Positive1_sucsses()//assignee change description 
        {
            //arrange
            //act
            taskService.UpdadeDescription("eldori@gmail.com", "HW", 0, "newDescription");
            //assert
            Assert.AreEqual("newDescription", boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(0).Description);
        }

        [Test, Order(28)]
        public void UpdateDescription_Positive2_sucsses()//assignee change description to null 
        {
            //arrange
            //act
            taskService.UpdadeDescription("eldori@gmail.com", "HW", 0, null);
            //assert
            Assert.AreEqual("newDescription", boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(0).Description);
        }

        [Test, Order(29)]
        public void UpdateDescription_Negative1_sucsses()//not assignee change description  
        {
            int boardID = boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW");
            taskService.UpdadeDescription("noa@gmail.com", "HW", 0, "notAssignee");
            Assert.AreEqual("newDescription", boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(0).Description);
        }

        [Test, Order(30)]
        public void UpdateDescription_Negative2_sucsses()//assignee change illegal description  
        {
            int boardID = boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW");
            taskService.UpdadeDescription("eldori@gmail.com", "HW", 0, " ");
            Assert.AreEqual(" ", boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(0).Description);
        }


        [Test, Order(31)]
        public void UpdateDescription_Negative3_sucsses()//assignee change illegal description  
        {
            int boardID = boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW");
            taskService.UpdadeDescription("eldori@gmail.com", "HW", 0, "ddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd");
            Assert.AreEqual(" ", boardService.GetBoardController().GetBoard(boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW")).GetTask(0).Description);
        }




        //limit column 
        [Test, Order(32)]
        public void LimitColumn_Positive()
        {
            int boardID = boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW");
            boardService.SetColumnLimit("noa@gmail.com", "HW", 0, 4);
            Assert.AreEqual(4, boardService.GetBoardController().GetBoard(boardID).columns[0].getMaxLength());  
        }

        [Test, Order(32)]
        public void LimitColumn_Positive1()
        {
            int boardID = boardService.GetBoardController().GetBoardIdByName("noa@gmail.com", "HW");
            boardService.SetColumnLimit("noa@gmail.com", "HW", 1, 0);
            Assert.AreEqual(0, boardService.GetBoardController().GetBoard(boardID).columns[1].getMaxLength());
        }






    }
}