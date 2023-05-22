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
    
    public class UserTests
    {
        public FactoryService factory;
        public UserService userService;
        BoardService boardService;
        TaskService taskService;

        [SetUp]
        public void Setup()
        {
            factory = new FactoryService();
            userService = factory.userService;
            boardService = factory.boardService;
            taskService = factory.taskService;
            factory.DeleteAllData();
        }

        [Test,Order(0)]
        public void RegisterTestPositive()
        {
            Assert.AreEqual("{}", userService.Register("eldor@gmail.com", "Eldor1212"));
            Assert.AreEqual("{}", userService.Register("Noanoa@gmail.com", "noA121212"));
        }

        [Test,Order(1)]
        public void Register_FalsePassword_Negative()
        {
            Assert.IsFalse(userService.Register("eldor@gmail.com", "eldor1212").Equals("{}"));// no capital letter
            Assert.IsFalse(userService.Register("eldor@gmail.com", "E123456").Equals("{}"));// no lowercase letter
            Assert.IsFalse(userService.Register("eldor@gmail.com", "El123").Equals("{}"));// less than 6 characters
            Assert.IsFalse(userService.Register("eldor@gmail.com", "El0123456789123456789").Equals("{}"));// more than 20 characters
            Assert.IsFalse(userService.Register("eldor@gmail.com", null).Equals("{}"));// null password
        }        

        [Test, Order(2)]
        public void Register_FalseEmail_Negative()
        {         
            Assert.IsFalse(userService.Register("eldor.gmail.com", "eldor1212").Equals("{}"));// [@ is not present]
            Assert.IsFalse(userService.Register("eldor@.gmail.com", "eldor1212").Equals("{}"));// [tld(Top Level domain) can not start with dot "."]
            Assert.IsFalse(userService.Register("@gmail.com", "eldor1212").Equals("{}"));// [No character before @ ]            
            Assert.IsFalse(userService.Register(".eldor@gmail.com", "eldor1212").Equals("{}"));//[an email should not be start with "."]
            Assert.IsFalse(userService.Register("eld()*or@gmail.com", "eldor1212").Equals("{}"));//[only character, digit, underscore, and dash are allowed in the domain]
            Assert.IsFalse(userService.Register("eldor@gmail..com", "eldor1212").Equals("{}"));//[double dots are not allowed]
            Assert.IsFalse(userService.Register("eldor@gmail.b", "eldor1212").Equals("{}"));//[".b" is not a valid tld]
        }

        [Test, Order(3)]
        public void Register_ExistingUser_Negative()
        {
            userService.Register("eldorm@post.bgu.ac.il", "Eldor12345");
            Assert.IsFalse(userService.Register("eldorm@post.bgu.ac.il", "Eldor12345").Equals("{}"));// check if a proper Response was accepted
            Assert.IsFalse(userService.getUserController().GetAllUsers().Count() == 2);// check that the user was not registered
        }

        [Test, Order(4)]
        public void LogIn_Positive()//log out positive is being tested here as well
        {
            userService.Register("eldorm@post.bgu.ac.il", "Eldor12345");
            Assert.IsTrue(userService.getUserController().GetUser("eldorm@post.bgu.ac.il").connected);// check if user is logged in after registration
            userService.LogOut("eldorm@post.bgu.ac.il");// log out the user
            Assert.IsFalse(userService.getUserController().GetUser("eldorm@post.bgu.ac.il").connected);//check if user is logged out
            userService.LogIn("eldorm@post.bgu.ac.il","Eldor12345");// log in again
            Assert.IsTrue(userService.getUserController().GetUser("eldorm@post.bgu.ac.il").connected);// check if user is logged in
        }

        [Test, Order(5)]
        public void LogIn_Negative()//log out positive is being tested here as well
        {
            userService.Register("eldorm@post.bgu.ac.il", "Eldor12345");
            userService.LogOut("eldorm@post.bgu.ac.il");// log out the user           
            Assert.IsFalse(userService.LogIn("eldorm@post.bgu.ac.il", "EyalGolanKaKi").Equals("{}"));// log in with false password
            Assert.IsFalse(userService.getUserController().GetUser("eldorm@post.bgu.ac.il").connected);//check the user is not logged in

            Assert.IsFalse(userService.LogIn("elrdm@post.bgu.ac.il", "Eldor12354").Equals("{}"));// log in with false email
            Assert.IsFalse(userService.getUserController().GetUser("eldorm@post.bgu.ac.il").connected);//check that user is not logged in

            userService.LogIn("eldorm@post.bgu.ac.il", "Eldor12345");
            Assert.IsFalse(userService.LogIn("eldorm@post.bgu.ac.il", "Eldor12345").Equals("{}"));// log in a connceted user should not be allowed
        }
        [Test, Order(6)]
        public void LogOut_Negative()
        {
            userService.Register("eldorm@post.bgu.ac.il", "Eldor12345");
            Assert.IsFalse(userService.LogOut("eldorm@post.bgu.ac").Equals("{}"));//log out a user using a wrong email
            Assert.IsTrue(userService.getUserController().GetUser("eldorm@post.bgu.ac.il").connected);//check the user is not logged out

            userService.LogOut("eldorm@post.bgu.ac.il");// log out the user
            Assert.IsFalse(userService.LogOut("eldorm@post.bgu.ac.il").Equals("{}"));// check a logged out user cannot be logged out again
        }

        [Test,Order(7)]
        public void ChangePassword_Positive()
        {
            userService.Register("eldorm@post.bgu.ac.il", "Eldor12345");

            userService.ChangePassword("eldorm@post.bgu.ac.il", "NewYearNewMe2022");// valid new password
            userService.LogOut("eldorm@post.bgu.ac.il");
            userService.LogIn("eldorm@post.bgu.ac.il", "NewYearNewMe2022");// log in with new password
            Assert.IsTrue(userService.getUserController().GetUser("eldorm@post.bgu.ac.il").connected);// check user is connected using its new password
        }

        [Test,Order(8)]
        public void ChangePassword_Negative()
        {
            userService.Register("eldorm@post.bgu.ac.il", "Eldor12345");

            userService.LogOut("eldorm@post.bgu.ac.il");
            Assert.IsFalse(userService.ChangePassword("eldorm@post.bgu.ac.il", "3Baboker").Equals("{}"));// check that a logged out user cannot change its password

            userService.LogIn("eldorm@post.bgu.ac.il", "Eldor12345");// logging user back in 
            Assert.IsFalse(userService.ChangePassword("eldorm@post.bgu.ac.il", "lotov10").Equals("{}"));// illegal password should be rejected
        }

        [Test, Order(9)]
        public void JoinLeaveBoard_Positive()
        {
            userService.Register("eldorm@post.bgu.ac.il", "Eldor12345");
            userService.Register("Omer@gmail.com", "Omer123123");
            boardService.CreateBoard("Omer@gmail.com", "Test board");
            int boardId = boardService.GetBoardController().GetBoardIdByName("Omer@gmail.com", "Test board");
            // check the user can join an existing board

            Assert.AreEqual("{}",userService.JoinBoard("eldorm@post.bgu.ac.il", boardId));//joining the board without errors
            Assert.AreEqual(2,boardService.GetBoardController().GetBoard(boardId).users.Count);// making sure the user has joined the board

            //removing the non owner user from the board
            Assert.AreEqual("{}", userService.LeaveBoard("eldorm@post.bgu.ac.il", "Test board"));// leaving the board without errors
            Assert.AreEqual(1,boardService.GetBoardController().GetBoard(boardId).users.Count);//making suer only the owner remains on the board           
        }

        [Test, Order(10)]
        public void JoinLeaveBoard_Negative()
        {
            userService.Register("eldorm@post.bgu.ac.il", "Eldor12345");
            userService.Register("Omer@gmail.com", "Omer123123");
            boardService.CreateBoard("Omer@gmail.com", "Test board");
            int boardId = boardService.GetBoardController().GetBoardIdByName("Omer@gmail.com", "Test board");

            //JoinBoard
            //trying to join board with false boardID
            Assert.False(userService.JoinBoard("eldorm@post.bgu.ac.il", boardId+1).Equals("{}"));// (this boardID does not exist in the system)
            Assert.AreEqual(1, boardService.GetBoardController().GetBoard(boardId).users.Count);// making sure the user has not joined the board

            //trying to join board with an invalid email
            Assert.False(userService.JoinBoard("eldorm@bgu.ac.il", boardId).Equals("{}"));
            Assert.AreEqual(1, boardService.GetBoardController().GetBoard(boardId).users.Count);// making sure the user has not joined the board

            //trying to join the same board twice
            Assert.AreEqual("{}", userService.JoinBoard("eldorm@post.bgu.ac.il", boardId));//joining the board without errors
            Assert.IsFalse(userService.JoinBoard("eldorm@post.bgu.ac.il", boardId).Equals("{}"));//trying go join the same board again(false)

            //LeaveBoard
            //trying to leave board with false boardName            
            Assert.IsFalse(userService.LeaveBoard("eldorm@post.bgu.ac.il", "Test Board").Equals("{}"));

            //trying to leave board with false email
            Assert.IsFalse(userService.LeaveBoard("eldorm@postbgu.ac.il", "Test board").Equals("{}"));

            //board owner trying to leave board
            Assert.IsFalse(userService.LeaveBoard("Omer@gmail.com", "Test board").Equals("{}"));
        }
        
        [Test, Order(11)]
        public void GetInProgress_Test()
        {
            //userService.Register("eldorm@post.bgu.ac.il", "Eldor12345");

            Assert.AreEqual("{}", userService.Register("Omer@gmail.com", "Omer123123"));          
            Assert.AreEqual("{}",boardService.CreateBoard("Omer@gmail.com", "Test board"));
            int boardId = boardService.GetBoardController().GetBoardIdByName("Omer@gmail.com", "Test board");

            Assert.AreEqual("{}", taskService.createTask("Omer@gmail.com", "Test board", "testMe", "", new DateTime(2022, 10, 12)));
            Assert.AreEqual("{}", taskService.AssignTask("Omer@gmail.com", "Omer@gmail.com", "Test board", 0));            
            Assert.AreEqual("{}", boardService.AdvanceTask("Omer@gmail.com", "Test board", 0));

            Assert.AreEqual("{}", taskService.createTask("Omer@gmail.com", "Test board", "testMe2", "", new DateTime(2022, 11, 12)));
            Assert.AreEqual("{}", taskService.AssignTask("Omer@gmail.com", "Omer@gmail.com", "Test board", 1));
            Assert.AreEqual("{}", boardService.AdvanceTask("Omer@gmail.com", "Test board", 1));

            Assert.AreEqual("{}", taskService.createTask("Omer@gmail.com", "Test board", "testMe3", "", new DateTime(2022, 12, 12)));
            Assert.AreEqual("{}", taskService.AssignTask("Omer@gmail.com", "Omer@gmail.com", "Test board", 2));
            Assert.AreEqual("{}", boardService.AdvanceTask("Omer@gmail.com", "Test board", 2));



            Console.WriteLine(userService.GetInProgress("Omer@gmail.com"));
        }
    }
}
