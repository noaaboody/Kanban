using NUnit.Framework;
using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace KanbanTestProj
{
    public class DataTests
    {
        FactoryService factory;

        [SetUp]
        public void Setup()
        {
            factory = new FactoryService();
            factory.DeleteAllData();
        }

        [Test]
        public void LoadDataDeleteData()
        {
            ///creation of a task in a board, with an and one differend user, and three tasks for three different columns
            factory.userService.Register("eldor@gmail.com", "Eldssssor10");
            factory.userService.Register("beyonce@gmail.com", "QuesssenB9");
            factory.boardService.CreateBoard("eldor@gmail.com", "eldor's board");
            int boardID = factory.boardService.GetBoardController().GetBoardIdByName("eldor@gmail.com", "eldor's board");
            //add tast No.1
            factory.userService.JoinBoard("beyonce@gmail.com", boardID);
            factory.taskService.createTask("beyonce@gmail.com", "eldor's board", "backLog test", "this task should be in BackLog column", new System.DateTime(2025, 04, 22));// TaskId = 0
            factory.taskService.AssignTask("beyonce@gmail.com", "beyonce@gmail.com", "eldor's board", 0);
            //add task No.2
            factory.taskService.createTask("eldor@gmail.com", "eldor's board", "In progress test", "this task should be in InProgress column", new System.DateTime(2025, 05, 23));// TaskId = 1
            factory.taskService.AssignTask("eldor@gmail.com", "eldor@gmail.com", "eldor's board", 1);
            factory.boardService.AdvanceTask("eldor@gmail.com", "eldor's board", 1);
            //add task NO.3             
            factory.taskService.createTask("eldor@gmail.com", "eldor's board", "Done test", "this task should be in Done column", new System.DateTime(2025, 06, 24));// TaskId = 2
            factory.taskService.AssignTask("eldor@gmail.com", "eldor@gmail.com", "eldor's board", 2);
            factory.boardService.AdvanceTask("eldor@gmail.com", "eldor's board", 2);
            factory.boardService.AdvanceTask("eldor@gmail.com", "eldor's board", 2);
               
            // reset all RAM data
            factory = new FactoryService();
               
            //Load all data from data base and see if the tasks exists in these columns
            System.Console.WriteLine(factory.LoadData());
            factory.userService.LogIn("beyonce@gmail.com", "QuesssenB9");
            System.Console.WriteLine("backLog column");
            System.Console.WriteLine(factory.boardService.GetColumn("beyonce@gmail.com", "eldor's board", 0));//should show task No.0
            System.Console.WriteLine("InProgress column");
            System.Console.WriteLine(factory.boardService.GetColumn("beyonce@gmail.com", "eldor's board", 1));//should show task No.1
            System.Console.WriteLine("Done column");
            System.Console.WriteLine(factory.boardService.GetColumn("beyonce@gmail.com", "eldor's board", 2));//should show task No.2
             
            // delete all Database data
            factory.DeleteAllData();
            Assert.AreEqual("{}", factory.userService.Register("eldor@gmail.com", "Eldor10"));//Register a user that existed in the previous data
            Assert.AreEqual("{}", factory.boardService.CreateBoard("eldor@gmail.com", "eldor's board")); // Add to that user a board with a name that existed
            System.Console.WriteLine(factory.boardService.GetColumn("eldor@gmail.com", "eldor's board", 0));//should show an empty column
            System.Console.WriteLine(factory.boardService.GetColumn("eldor@gmail.com", "eldor's board", 1));//should show an empty column
            System.Console.WriteLine(factory.boardService.GetColumn("eldor@gmail.com", "eldor's board", 2));//should show an empty column
        }
    }
}