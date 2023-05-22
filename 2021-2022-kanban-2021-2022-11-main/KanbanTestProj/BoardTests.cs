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
    
    public class BoardTests
    {
        FactoryService factory;
        [Test, Order(0)]
        public void Setup()
        {
            factory = new FactoryService();
            factory.DeleteAllData();
            
        }

        [Test,Order(1)]
        public void TestShouldFailCreateBoard1()
        {
            string email = "eld222or@gmail.com";  
            factory.userService.Register(email, "Eldor1212");
            factory.boardService.CreateBoard(email, "    ");


         
                Assert.AreEqual(factory.boardService.GetBoardController().boards.ContainsKey(0), false);
            
        }

        [Test,Order(2)]
        public void TestShouldFailCreateBoard2()//cheks if the item name mathes the one in the constructor
        {
            string email = "eld222or@gmail.com";
                factory.boardService.CreateBoard(" ", "nn");
                Assert.AreEqual(factory.boardService.GetBoardController().boards.ContainsKey(0), false);
            
        }

        [Test, Order(3)]
        public void TestShouldFailCreateBoard3()//cheks if the item name mathes the one in the constructor
        {
                string email=null;
                factory.boardService.CreateBoard(email, "nn");
                Assert.AreEqual(factory.boardService.GetBoardController().boards.ContainsKey(0), false);


        }

        [Test, Order(4)]
        public void TestShouldFailCreateBoard4()//cheks if the item name mathes the one in the constructor
        {
            
            
                string email = "eld222or@gmail.com"; ;
                factory.boardService.CreateBoard(email, null);
                Assert.AreEqual(factory.boardService.GetBoardController().boards.ContainsKey(0), false);


        }

        [Test, Order(5)]
        public void TestShouldPassCreateBoard1()//cheks if the item name mathes the one in the constructor
        {
            string email = "eld222or@gmail.com";
            factory.userService.Register(email, "Eldor1212");
            factory.boardService.CreateBoard(email, "Board1");
            Assert.AreEqual(factory.boardService.GetBoardController().boards.ContainsKey(0), true);

        }

        [Test, Order(6)]
        public void TestShouldFailCreateBoard5()//cheks if the item name mathes the one in the constructor
        {
     
                string email = "eld222or@gmail.com"; ;
                factory.boardService.CreateBoard(email, "Board1");
                Assert.AreEqual(factory.boardService.GetBoardController().boards.Count(),1);


        }
        [Test, Order(7)]

        public void TestShouldFailCreateBoard6()//cheks if the item name mathes the one in the constructor
        {

            factory.boardService.CreateBoard("omer10828@gmail.com", "Board1");
            Assert.AreEqual(factory.boardService.GetBoardController().boards.Count(), 1);


        }

        [Test,Order(8)]
        public void TestShouldPassCreateBoard2()//should fail because it has no tasks
        {
            factory.boardService.CreateBoard("eld222or@gmail.com", "Board7");
            Assert.AreEqual(factory.boardService.GetBoardController().boards.Count(), 2);
        }

        [Test, Order(9)]
        public void AdvanceTaskTest1Pass()
        {
        
            factory.boardService.GetBoardController().boards[0].AddTask("feed", "feed", DateTime.Now.AddDays(10));
            factory.boardService.AdvanceTask("eld222or@gmail.com", "Board1", 0);
            Assert.AreEqual(factory.boardService.GetBoardController().boards[0].GetColumn(1).Count(), 1);
        }

        [Test, Order(10)]
        public void AdvanceTaskTest2Pass()
        {
            factory.boardService.AdvanceTask("eld222or@gmail.com", "Board1", 0);
            Assert.AreEqual(factory.boardService.GetBoardController().boards[0].GetColumn(2).Count(), 1);
        }

        [Test, Order(11)]

        public void AdvanceTaskTest3Pass()
        {
            factory.taskService.AssignTask("eld222or@gmail.com", "eld222or@gmail.com", "Board1", 0);
            factory.boardService.AdvanceTask("eld222or@gmail.com", "Board1", 0);
            Assert.AreEqual(factory.boardService.GetBoardController().boards[0].GetColumn(2).Count(), 1);
        }

        [Test, Order(12)]
        public void SetColumnLimitTest1Pass()
        {
            factory.boardService.SetColumnLimit("eld222or@gmail.com", "Board1", 0, 13);
            Assert.AreEqual(factory.boardService.GetBoardController().boards[0].GetColumnLimit(0), 13);
        }


        [Test, Order(13)]
        public void SetColumnLimitTest2Pass()
        {
            factory.boardService.SetColumnLimit("eld222or@gmail.com", "Board1", 2, 3);
            Assert.AreEqual(factory.boardService.GetBoardController().boards[0].GetColumnLimit(2), 3);
        }

        [Test, Order(14)]
        public void SetColumnLimitTest3Pass()
        {
            factory.boardService.SetColumnLimit("eld222or@gmail.com", "Board1", 2, 1);
            Assert.AreEqual(factory.boardService.GetBoardController().boards[0].GetColumnLimit(2), 1);
        }

        [Test, Order(15)]
        public void SetColumnLimitTest4Pass()
        {
            factory.boardService.SetColumnLimit("eld222or@gmail.com", "Board1", 0, -1);
            Assert.AreEqual(factory.boardService.GetBoardController().boards[0].GetColumnLimit(1), -1);
        }

        [Test, Order(16)]
        public void SetColumnLimitTest5Pass()
        {
            factory.boardService.SetColumnLimit("Omer10@gmail.com", "Board1", 0, 14);
            Assert.AreEqual(factory.boardService.GetBoardController().boards[0].GetColumnLimit(0), -1);
        }

        [Test, Order(17)]
        public void GetUserBoards1()//see test on  to see output of writeline
        {
            factory.boardService.CreateBoard("eld222or@gmail.com", "BB");
            Console.WriteLine(factory.boardService.GetUserBoards("eld222or@gmail.com"));//shoulde have id's:0,1,2 as board id's.
            Console.WriteLine("shoulde have id's:0,1,2 as board id's");
        }
        [Test, Order(18)]
        public void RmoveBoard1()//see test on  to see output of writeline
        {
            factory.boardService.RemoveBoard("eld222or@gmail.com", "BB");
            Console.WriteLine(factory.boardService.GetUserBoards("eld222or@gmail.com"));//shoulde have id's:0,1 as board id's.
            Console.WriteLine("shoulde have id's:0,1 as board id's.");
        }
        [Test, Order(19)]

        public void RmoveBoard2()//see test on  to see output of writeline
        {
            factory.boardService.RemoveBoard("2or@gmail.com", "Board1");
            Console.WriteLine(factory.boardService.GetUserBoards("eld222or@gmail.com"));
            Console.WriteLine("shoulde have id's:0,1 as board id's");//.

        }

        [Test, Order(20)]
        public void RmoveBoard3()//see test on  to see output of writeline
        {
            factory.boardService.RemoveBoard("eld222or@gmail.com", "Board1");
            factory.boardService.RemoveBoard("eld222or@gmail.com", "Board7");
            Console.WriteLine(factory.boardService.GetUserBoards("eld222or@gmail.com"));//shoulde have id's:[] as board id's.
            Console.WriteLine("shoulde have id's:[] as board id's.");
        }
        [Test, Order(21)]

        public void RmoveBoard4()//see test on  to see output of writeline
        {
            factory.boardService.RemoveBoard("eld222or@gmail.com", "Board1");
            factory.boardService.RemoveBoard("eld222or@gmail.com", "Board7");
            Console.WriteLine(factory.boardService.GetUserBoards("eld222or@gmail.com"));//shoulde have id's[] as board id's.
            Console.WriteLine("shoulde have id's:[] as board id's.");
        }

        [Test, Order(22)]

        public void GetColumn1()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";
            factory.boardService.CreateBoard(email, "Board1");
            factory.boardService.GetBoardController().boards[3].AddTask("0", "0", DateTime.Now.AddDays(10));
            factory.boardService.GetBoardController().boards[3].AddTask("1", "1", DateTime.Now.AddDays(10));
            factory.boardService.GetBoardController().boards[3].AddTask("2", "2", DateTime.Now.AddDays(10));
            factory.boardService.AdvanceTask("eld222or@gmail.com", "Board1", 0);




            Console.WriteLine(factory.boardService.GetColumn(email,"Board1",0));//shoulde have id's[] as board id's.
            Console.WriteLine("shoulde have 2 taks");
        }

        [Test, Order(23)]

        public void GetColumn2()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";

            Console.WriteLine(factory.boardService.GetColumn(email, "Board1", 1));//shoulde have id's[] as board id's.
            Console.WriteLine("shoulde have 1 taks");
        }


        [Test, Order(24)]

        public void GetColumn3()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";

            Console.WriteLine(factory.boardService.GetColumn(email, "Board1", 1));//shoulde have id's[] as board id's.
            Console.WriteLine("shoulde have 0 taks");
        }

        [Test, Order(25)]

        public void GetColumn4()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";

            Console.WriteLine(factory.boardService.GetColumn(email, "Board1", 4));//shoulde have id's[] as board id's.
            Console.WriteLine("shoulde print illegal column ordinal error");
        }

        [Test, Order(26)]

        public void GetColumnName1()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";

            Console.WriteLine(factory.boardService.GetColumnName(email, "Board1", 0));//shoulde have id's[] as board id's.
            Console.WriteLine("shoulde print BackLog");
        }

        [Test, Order(27)]

        public void GetColumnName2()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";

            Console.WriteLine(factory.boardService.GetColumnName(email, "Board1", 1));//shoulde have id's[] as board id's.
            Console.WriteLine("shoulde print InProgress");
        }
        [Test, Order(28)]

        public void GetColumnName3()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";

            Console.WriteLine(factory.boardService.GetColumnName(email, "Board1", 2));//shoulde have id's[] as board id's.
            Console.WriteLine("shoulde print Done");
        }

        [Test, Order(29)]

        public void GetColumnName4()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";

            Console.WriteLine(factory.boardService.GetColumnName(email, "Board1",4));//shoulde have id's[] as board id's.
            Console.WriteLine("shoulde print illegal column ordinal error");
        }


        [Test, Order(30)]

        public void GetColumnName5()//see test on  to see output of writeline
        {//checkiii
            string email = "eld222or@gmail.com";

            Console.WriteLine(factory.boardService.GetColumnName(email, "Board1", 4));//shoulde have id's[] as board id's.
            Console.WriteLine("shoulde print illegal column ordinal error");
        }

        [Test, Order(31)]

        public void GetColumnLimit1()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";

            Console.WriteLine(factory.boardService.GetColumnLimit(email, "Board1", 0));//shoulde have id's[] as board id's.
            Console.WriteLine("-1");
        }

        [Test, Order(32)]

        public void GetColumnLimit2()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";
            factory.boardService.SetColumnLimit(email, "Board1", 1, 5);

            Console.WriteLine(factory.boardService.GetColumnLimit(email, "Board1", 1));//shoulde have id's[] as board id's.
            Console.WriteLine("should print 5");
        }

        [Test, Order(33)]

        public void GetColumnLimit3()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";
            factory.boardService.SetColumnLimit(email, "Board1", 1, -1);

            Console.WriteLine(factory.boardService.GetColumnLimit(email, "Board1", 1));//shoulde have id's[] as board id's.
            Console.WriteLine(" should print -1");
        }


        [Test, Order(34)]

        public void GetColumnLimit4()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";
            factory.boardService.SetColumnLimit(email, "Board1", 1, 5);

            Console.WriteLine(factory.boardService.GetColumnLimit(email, "Board1", 1));//shoulde have id's[] as board id's.
            Console.WriteLine("should print 5");
        }

        [Test, Order(35)]

        public void GetColumnLimit5()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";
            factory.boardService.SetColumnLimit(email, "Board1", 1, 5);

            Console.WriteLine(factory.boardService.GetColumnLimit(email, "Board1", -1));//shoulde have id's[] as board id's.
            Console.WriteLine("should print illegal column ordinal error");
        }

        [Test, Order(36)]

        public void GetBoardName1()//see test on  to see output of writeline
        {
            

            Console.WriteLine(factory.boardService.GetBoardName(3));//shoulde have id's[] as board id's.
            Console.WriteLine("should print Board1");
        }

        [Test, Order(37)]

        public void GetBoardName2()//see test on  to see output of writeline
        {


            Console.WriteLine(factory.boardService.GetBoardName(4));//shoulde have id's[] as board id's.
            Console.WriteLine("should print Dictionary key error");
        }

        [Test, Order(38)]

        public void RemoveUserFromBoard()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";
            string email2 = "Omer222@gmail.com";
            factory.userService.Register(email2, "kKKpop2lll89");
            factory.userService.JoinBoard(email2, 3);
            Console.WriteLine(factory.boardService.GetUserBoards(email2));//shoulde have id's[] as board id's.
            Console.WriteLine("should print 3 as part of a list Before user Removal");
            factory.boardService.RemoveUser(email2, 3);

            Console.WriteLine(factory.boardService.GetUserBoards(email2));//shoulde have id's[] as board id's.
            Console.WriteLine("should print an empty list after User Removal");

        }

        [Test, Order(39)]

        public void RemoveUserFromBoard2()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";
            string email2 = "Omer222@gmail.com";

            Console.WriteLine( factory.boardService.RemoveUser(email2, 3));

            Console.WriteLine("should print that user is not in board error");

        }

        [Test, Order(40)]

        public void ChangeOwner1()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";
            string email2 = "omer222@gmail.com";

            factory.userService.JoinBoard(email2, 3);

            factory.boardService.TransferOwnership(email, email2, "Board1");
            Assert.AreEqual( factory.boardService.GetBoardController().GetBoard(3).owner,email2);

            

        }


        [Test, Order(41)]

        public void ChangeOwner2()//see test on  to see output of writeline
        {
            string email = "eld222or@gmail.com";
            string email2 = "omer222@gmail.com";

            

            factory.boardService.TransferOwnership(email2, "Liav88@walla.com", "Board1");
            Assert.AreEqual(factory.boardService.GetBoardController().GetBoard(3).owner, email2);



        }









    }
}
