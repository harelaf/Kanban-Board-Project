using Microsoft.VisualStudio.TestTools.UnitTesting;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Collections.Generic;
using System.Linq;
using System;

namespace KanbanTesting
{
    [TestClass]
    public class MainTest
    {
        private IService Service;
        private TestUser User1;
        private TestUser User2;
        private TestUser User3;
        private TestUser User4;
        private TestTask Task1;
        private TestUser LoggedInUser;
        private readonly string[] columnsNames = new string[] { "backlog", "in progress", "done" };




        /*****************************************************************************************************
                                            Test 1 : Good User Jerusalem
          ---------------------------------------------------------------------------------------------------
                 This test tests very simple function calls - all the calls should be successful
         *****************************************************************************************************/


        [TestMethod]
        public void Test1()
        {
            Service = new Service();
            Service.LoadData();
            User1 = new TestUser("user1@gmail.com", "MyPass123", "myCoolName");
            User2 = new TestUser("user2@gmail.com", "MyPass321", "user4ever");
            Task1 = new TestTask("title1", "description1", new DateTime(2021, 1, 1));

            // Please don't change the order of the calls
            TestUsersFunctions1();
            TestBoardFunctions1();
            TestTasksAndColumnsFunctions1();
        }

        /*----------------------------------------------------------------*/

        private void TestUsersFunctions1()
        {
            // Please don't change the order of the calls
            SuccessfulRegisterUser(User2);
            SuccessfulRegisterUser(User1);
            SuccessfulLoginUser(User2);
            SuccessfulLogoutUser(User2);
            SuccessfulLoginUser(User1);
        }

        private void SuccessfulRegisterUser(TestUser user)
        {
            Response response = Service.Register(user.Email, user.Password, user.Nickname);
            Assert.IsFalse(response.ErrorOccured, "registering with valid arguments");
        }

        private void SuccessfulLoginUser(TestUser user)
        {
            Response<User> response = Service.Login(user.Email, user.Password);
            Assert.IsFalse(response.ErrorOccured, "Login with valid arguments");
            Assert.IsNotNull(response.Value, "should return the Logged in user");
            Assert.AreEqual(response.Value.Email, user.Email);
            Assert.AreEqual(response.Value.Nickname, user.Nickname);
        }

        private void SuccessfulLogoutUser(TestUser user)
        {
            Response response = Service.Logout(user.Email);
            Assert.IsFalse(response.ErrorOccured, "Logout for Logged in user");
        }

        /*----------------------------------------------------------------*/

        private void TestBoardFunctions1()
        {
            SuccessfulGetBoard(User1);
        }

        private void SuccessfulGetBoard(TestUser user)
        {
            Response<Board> response = Service.GetBoard(user.Email);

            //test legal response:
            Assert.IsFalse(response.ErrorOccured, "get board of Logged in user");
            Assert.IsNotNull(response.Value, "should return the Logged in users board");

            //compare the columns names:
            var columnOutputNames = response.Value.ColumnsNames;
            var expectedAndOutput = columnsNames.Zip(columnOutputNames, (o, e) => new { Output = o, Expected = e });
            foreach (var columnName in expectedAndOutput)
                Assert.AreEqual(columnName.Output, columnName.Expected, $"the first column name - expected: '{columnName.Expected}', output: '{columnName.Output}' ");
        }

        /*----------------------------------------------------------------*/

        private void TestTasksAndColumnsFunctions1()
        {
            SuccessfulLimitColumnTask_Backlog_3Tasks_User1();
            int task1Id = SuccessfulAddTask_And_LookUpFromColumn_Task1_User1();
            SuccessfulUpdateTaskTitle_Task1_User1(task1Id);
            SuccessfulUpdateTaskDescription_Task1_User1(task1Id);
            SuccessfulUpdateTaskDueDate_Task1_User1(task1Id);
            SuccessfulAdvanceTask_FromBacklog_Task1_User1(task1Id);
        }

        private void SuccessfulLimitColumnTask_Backlog_3Tasks_User1()
        {
            Response response = Service.LimitColumnTasks(User1.Email, 0, 3);
            Assert.IsFalse(response.ErrorOccured, $"successful limit column task to Logged in user. \nYourError: {response.ErrorMessage}");
        }

        private int SuccessfulAddTask_And_LookUpFromColumn_Task1_User1()
        {
            Response<Task> responseTask = Service.AddTask(User1.Email, Task1.Title, Task1.Description, Task1.DueDate);
            Assert.IsFalse(responseTask.ErrorOccured, $"successful add task to Logged in user. \nYourError: {responseTask.ErrorMessage}");
            Assert.IsNotNull(responseTask.Value, "should return the task. instead null returned");
            Assert.AreEqual(responseTask.Value.Title, Task1.Title, $"Title not match. Expected: '{Task1.Title}'. Output: '{responseTask.Value.Title}'");
            Assert.AreEqual(responseTask.Value.Description, Task1.Description, $"Description not match. Expected: '{Task1.Description}'. Output: '{responseTask.Value.Description}'");
            Assert.AreEqual(responseTask.Value.DueDate, Task1.DueDate, $"DueDate not match. Expected: '{Task1.DueDate}'. Output: '{responseTask.Value.DueDate}'");
            Assert.AreEqual(responseTask.Value.CreationTime.ToShortDateString(), DateTime.Now.ToShortDateString(),
                              $"CreationTime not match. Expected: '{Task1.DueDate.ToShortDateString()}'. Output: '{responseTask.Value.DueDate.ToShortDateString()}'"); //I hope this gonna work

            int taskId = responseTask.Value.Id;
            Response<Column> responseColumn = Service.GetColumn(User1.Email, 0);
            Assert.IsFalse(responseColumn.ErrorOccured, $"successful getColumn(int) to Logged in user. \nYourError: {responseColumn.ErrorMessage}");
            Assert.IsNotNull(responseTask.Value, "should return the column. instead null returned");
            Assert.AreEqual(responseColumn.Value.Limit, 3, $"limit task has changed in previous test to 3 tasks. Output: {responseColumn.Value.Limit}");

            string taskTitleFromColumn = responseColumn.Value.Tasks.FirstOrDefault(task => task.Id == taskId).Title;
            Assert.AreEqual(responseTask.Value.Title, taskTitleFromColumn, "task returned from AddTask operation is different (different title) from the task in the backlog column");
            return taskId;
        }

        private void SuccessfulUpdateTaskTitle_Task1_User1(int taskId)
        {
            Task1.Title = "NewTitle1";
            Response response = Service.UpdateTaskTitle(User1.Email, 0, taskId, Task1.Title);
            Assert.IsFalse(response.ErrorOccured, $"successful UpdateTaskTitle to Logged in user. \nYourError: {response.ErrorMessage}");
            Response<Column> responseColumn = Service.GetColumn(User1.Email, columnsNames[0]);
            Assert.IsFalse(responseColumn.ErrorOccured, $"successful getColumn(name) to Logged in user. \nYourError: {responseColumn.ErrorMessage}");
            Assert.IsNotNull(responseColumn.Value, "should return the column. instead null returned");
            string UpdatedTaskTitle = responseColumn.Value.Tasks.FirstOrDefault(task => task.Id == taskId).Title;
            Assert.AreEqual(Task1.Title, UpdatedTaskTitle, $"task's title from getColumn is different from expected. Output: {UpdatedTaskTitle}. Expected: {Task1.Title}");
        }

        private void SuccessfulUpdateTaskDescription_Task1_User1(int taskId)
        {
            Task1.Description = "NewDescription1";
            Response response = Service.UpdateTaskDescription(User1.Email, 0, taskId, Task1.Description);
            Assert.IsFalse(response.ErrorOccured, $"successful UpdateTaskDescription to Logged in user. \nYourError: {response.ErrorMessage}");
            Response<Column> responseColumn = Service.GetColumn(User1.Email, columnsNames[0]);
            string UpdatedTaskDescription = responseColumn.Value.Tasks.FirstOrDefault(task => task.Id == taskId).Description;
            Assert.AreEqual(Task1.Description, UpdatedTaskDescription, $"task's Description from getColumn is different from expected. Output: {UpdatedTaskDescription}. Expected: {Task1.Description}");
        }

        private void SuccessfulUpdateTaskDueDate_Task1_User1(int taskId)
        {
            Task1.DueDate = new DateTime(2022, 1, 1);
            Response response = Service.UpdateTaskDueDate(User1.Email, 0, taskId, Task1.DueDate);
            Assert.IsFalse(response.ErrorOccured, $"successful UpdateTaskDueDate to Logged in user. \nYourError: {response.ErrorMessage}");
            Response<Column> responseColumn = Service.GetColumn(User1.Email, columnsNames[0]);
            DateTime UpdatedTaskDueDate = responseColumn.Value.Tasks.FirstOrDefault(task => task.Id == taskId).DueDate;
            Assert.AreEqual(Task1.DueDate, UpdatedTaskDueDate, $"task's DueDate from getColumn is different from expected. Output: {UpdatedTaskDueDate}. Expected: {Task1.DueDate}");
        }

        private void SuccessfulAdvanceTask_FromBacklog_Task1_User1(int taskId)
        {
            Response response = Service.AdvanceTask(User1.Email, 0, taskId);
            Assert.IsFalse(response.ErrorOccured, $"successful AdvanceTask to Logged in user. \nYourError: {response.ErrorMessage}");
            Response<Column> backlogCol = Service.GetColumn(User1.Email, 0);
            Response<Column> inProgressCol = Service.GetColumn(User1.Email, 1);
            Assert.IsTrue(backlogCol.Value.Tasks.Count == 0, $"backlog column should have one task. Output: {backlogCol}");
            Assert.IsTrue(inProgressCol.Value.Tasks.Count == 1, $"in progress column should have one task. Output: {inProgressCol}");
        }




        /*****************************************************************************************************
                                            Test 2 : Classic User
          ---------------------------------------------------------------------------------------------------
              This test tests the implementation of simple requirements with valid and invalid input
         *****************************************************************************************************/




        [TestMethod]
        public void Test2()
        {
            Service = new Service();
            Service.LoadData();
            User1 = new TestUser("user1@gmail.com", "MyPass123", "myCoolName");
            User2 = new TestUser("user2@gmail.com", "MyPass321", "user4ever");
            User3 = new TestUser("user3@gmail.com", "Abc1", "nickname3");
            User4 = new TestUser("user4@gmail.com", "AAAAAbbbbbccccc12345", "nickname4");

            // Please don't change the order of the calls
            TestUsersFunctions2();
            TestBoardFunctions2();
            TestTasksAndColumnsFunctions2();
        }

        /*----------------------------------------------------------------*/

        private void TestUsersFunctions2()
        {
            // Please don't change the order of the calls
            SuccessfulLogin_User2();
            SuccessfulLogout_User2();
            SuccessfulRegistration_4CharPassword_User3();
            SuccessfulRegistration_20CharPassword_User4();
            FailRegistration_Under4CharsPassword();
            FailRegistration_Above20CharsPassword();
            FailRegistration_NoUpperCharPassword();
            FailRegistration_NoLowerCharPassword();
            FailRegistration_NoNumbersPassword();
            FailLogin_NotExistingEmail();
            FailLogin_NotMatchPassword();
            FailRegistration_RegisterForRegisteredUser();
            SuccessfulLogin_User1();
            LoggedInUser = User1;
        }

        private void SuccessfulLogin_User2()
        {
            Response<User> response = Service.Login(User2.Email, User2.Password);
            Assert.IsFalse(response.ErrorOccured, "the previous test exit with Logged in user. The loading of the data not suppose to load the Logged in user.\nAnother option is that the loadData failed load the users");
            Assert.AreEqual(response.Value.Email, User2.Email);
            Assert.AreEqual(response.Value.Nickname, User2.Nickname);
        }

        private void SuccessfulLogout_User2()
        {
            Response response = Service.Logout(User2.Email);
            Assert.IsFalse(response.ErrorOccured, "Logout for Logged in user");
        }

        private void SuccessfulRegistration_4CharPassword_User3()
        {
            Response response = Service.Register(User3.Email, User3.Password, User3.Nickname);
            Assert.IsFalse(response.ErrorOccured, $"user can create 4 chars legal password. \nYourError: {response.ErrorMessage}");
        }

        private void SuccessfulRegistration_20CharPassword_User4()
        {
            Response response = Service.Register(User4.Email, User4.Password, User4.Nickname);
            Assert.IsFalse(response.ErrorOccured, $"user can create 20 chars legal password. \nYourError: {response.ErrorMessage}");
        }

        private void FailRegistration_Under4CharsPassword()
        {
            Response response = Service.Register("user@gmail.com", "Ab1", "nick");
            Assert.IsTrue(response.ErrorOccured, "should fail if the password is too short");
        }

        private void FailRegistration_Above20CharsPassword()
        {
            Response response = Service.Register("user@gmail.com", "AAAAAbbbbbccccc12345toolong", "nick");
            Assert.IsTrue(response.ErrorOccured, "should fail if the password is too long");
        }

        private void FailRegistration_NoUpperCharPassword()
        {
            Response response = Service.Register("user@gmail.com", "2badpassword", "nick");
            Assert.IsTrue(response.ErrorOccured, "should fail if the password not contains UpperCase letters");
        }

        private void FailRegistration_NoLowerCharPassword()
        {
            Response response = Service.Register("user@gmail.com", "CAPS1LOCK1ON", "nick");
            Assert.IsTrue(response.ErrorOccured, "should fail if the password not contains LowerCase letters");
        }

        private void FailRegistration_NoNumbersPassword()
        {
            Response response = Service.Register("user@gmail.com", "IDontLikeMath", "nick");
            Assert.IsTrue(response.ErrorOccured, "should fail if the password not contains numbers");
        }

        private void FailLogin_NotExistingEmail()
        {
            Response<User> response = Service.Login("wrongemail@jmail.bom", "wrongPass1O1");
            Assert.IsTrue(response.ErrorOccured, "not existing email");
        }

        private void FailLogin_NotMatchPassword()
        {
            Response<User> response = Service.Login(User1.Email, "HackMyFriend666");
            Assert.IsTrue(response.ErrorOccured, "the password not matches the email");
        }

        private void FailRegistration_RegisterForRegisteredUser()
        {
            Response response = Service.Register(User1.Email, User1.Password, User1.Nickname);
            Assert.IsTrue(response.ErrorOccured, "the user probbably register instead of Login. user cannot register with existing email");
        }

        private void SuccessfulLogin_User1()
        {
            Response<User> response = Service.Login(User1.Email, User1.Password);
            Assert.AreEqual(response.Value.Email, User1.Email);
            Assert.AreEqual(response.Value.Nickname, User1.Nickname);
        }

        /*----------------------------------------------------------------*/
        private void TestBoardFunctions2()
        {
            FailGetBoard_NotLoggedInUser();
        }

        private void FailGetBoard_NotLoggedInUser()
        {
            Response<Board> response = Service.GetBoard(User3.Email);
            Assert.IsTrue(response.ErrorOccured, "not Logged in user can't access his board - check the email in Getboard");
        }

        /*----------------------------------------------------------------*/

        private void TestTasksAndColumnsFunctions2()
        {
            // Please don't change the order of the calls
            FailAddTask_EmptyTitle();
            SuccessfulAddTask_EmptyDescription();
            //Summary: 1 task in the Backlog and 1 in the InProgress
            SuccessfulLimitColumnTask_BacklogToNoLimit();
            SuccessfulLimitColumnTask_BacklogTo1Task();
            FailAddTask_BacklogIsfull();
            SuccessfulLimitColumnTask_InProgressTo1Task();
            FailAdvanceTask_FromBacklogToFullInProgress();
            SuccessfulUpdateTaskDueDate_FromInProgress_LegalDueDate();
            SuccessfulAdvanceTask_FromInProgress();
            FailUpdateTaskTitle_FromDoneColumn();
        }

        private void FailAddTask_EmptyTitle()
        {
            Response<Task> response = Service.AddTask(LoggedInUser.Email, "", "I don't want to give title", new DateTime(2021, 6, 6));
            Assert.IsTrue(response.ErrorOccured, "Title can't be empty");
        }

        private void SuccessfulAddTask_EmptyDescription()
        {
            Response<Task> response = Service.AddTask(LoggedInUser.Email, "Finish Milestone 2!!", "", new DateTime(2020, 10, 10)); //I wish this was the due date
            Assert.IsFalse(response.ErrorOccured, $"Description can be empty, \nYourError: {response.ErrorMessage}");
        }

        private void SuccessfulLimitColumnTask_BacklogToNoLimit()
        {
            Response response = Service.LimitColumnTasks(LoggedInUser.Email, 0, -1);
            Assert.IsFalse(response.ErrorOccured, "limit column tasks to -1 (no limit). " +
                "you probably check 'is the newLimit < tasks.size?' and need to add 'AND newLimit != -1'. " +
                $"\nYourError: {response.ErrorMessage}");
        }

        private void SuccessfulLimitColumnTask_BacklogTo1Task()
        {
            Response response = Service.LimitColumnTasks(LoggedInUser.Email, 0, 1);
            Assert.IsFalse(response.ErrorOccured, $"limit backlog column to 1 (1 task in the column). \nYourError: {response.ErrorMessage}");
        }

        private void FailAddTask_BacklogIsfull()
        {
            Response<Task> response = Service.AddTask(LoggedInUser.Email, "Find a vaccine for the Coronavirus", "important", new DateTime(2020, 7, 7));
            Assert.IsTrue(response.ErrorOccured, "task can't be added to a full backlog");
        }

        private void SuccessfulLimitColumnTask_InProgressTo1Task()
        {
            Response response = Service.LimitColumnTasks(LoggedInUser.Email, 1, 1);
            Assert.IsFalse(response.ErrorOccured, $"limit in progress column to 1 (1 task in the column). \nYourError: {response.ErrorMessage}");
        }

        private void FailAdvanceTask_FromBacklogToFullInProgress()
        {
            Response<Column> responseColumn = Service.GetColumn(LoggedInUser.Email, 0);
            Response response = Service.AdvanceTask(LoggedInUser.Email, 0, responseColumn.Value.Tasks.FirstOrDefault().Id);
            Assert.IsTrue(response.ErrorOccured, "task can't be advance to a full column (in progress)");
        }

        private void SuccessfulUpdateTaskDueDate_FromInProgress_LegalDueDate()
        {
            Response<Column> responseColumn = Service.GetColumn(LoggedInUser.Email, 1);
            Response response = Service.UpdateTaskDueDate(LoggedInUser.Email, 1, responseColumn.Value.Tasks.FirstOrDefault().Id, new DateTime(2021, 7, 3));
            Assert.IsFalse(response.ErrorOccured, $"update task from in progress to with new DueDate. \nYourError: {response.ErrorMessage}");
        }

        private void SuccessfulAdvanceTask_FromInProgress()
        {
            Response<Column> responseColumn = Service.GetColumn(LoggedInUser.Email, 1);
            Response response = Service.AdvanceTask(LoggedInUser.Email, 1, responseColumn.Value.Tasks.FirstOrDefault().Id);
            Assert.IsFalse(response.ErrorOccured, $"successful AdvanceTask from inProgress to done column. \nYourError: {response.ErrorMessage}");
        }

        private void FailUpdateTaskTitle_FromDoneColumn()
        {
            Response<Column> responseColumn = Service.GetColumn(LoggedInUser.Email, 2);
            Response response = Service.UpdateTaskTitle(LoggedInUser.Email, 2, responseColumn.Value.Tasks.FirstOrDefault().Id, "new title for done task");
            Assert.IsTrue(response.ErrorOccured, "update task from done column is forbidden Operation");
        }




        /*********************************************************************************************************************
                                            Test 3 : Annoying User
          ---------------------------------------------------------------------------------------------------
           This test tests annoying cases of invalid arguments and expect the program to handle it properly

         IMPORTANT: You may pass this tests easily - pay attention that your service is probably catching ALL the exceptions.
         The test CANT identify whether the caught exception is thrown form YOUR code or from the SYSTEM.
         I suggest you to debug this test and check manually this issue 
         OR 
         check your LOG FILE to find out
         OR
         Watch the last minutes of Achia (!?) Office hours (19.4) - When he explained "MyException"
         ********************************************************************************************************************/




        [TestMethod]
        public void Test3()
        {
            Service = new Service();
            Service.LoadData();
            User3 = new TestUser("user3@gmail.com", "Abc1", "nickname3");
            User4 = new TestUser("user4@gmail.com", "AAAAAbbbbbccccc12345", "nickname4");

            // Please don't change the order of the calls
            TestUsersFunctions3();
            TestTasksAndColumnsFunctions3();
        }

        /*----------------------------------------------------------------*/

        private void TestUsersFunctions3()
        {
            // Please don't change the order of the calls
            TestOverrideRegistrationScenario(); //after scenario - user 4 is Logged in
            LoggedInUser = User4;
            FailLogin_UserTryToLoginWhileLoggedIn();
            FailLogin_OtherUserTryToLoginWhileLoggedIn();

        }

        private void TestOverrideRegistrationScenario()
        {
            FailRegister_ExistingEmailAndWrongPassword();
            FailLogin_ExistingEmailAndSameWrongPassword();
            SuccessfulLogin_ExistingEmailAndOldRightPassword();
        }
        private void FailRegister_ExistingEmailAndWrongPassword()
        {
            Response response = Service.Register(User4.Email, "OverrideYourPass123", "nickname4");
            Assert.IsTrue(response.ErrorOccured, "should fail because email address belongs to user already (make sure you have done the previous tests)");
        }
        private void FailLogin_ExistingEmailAndSameWrongPassword()
        {
            Response<User> response = Service.Login(User4.Email, "OverrideYourPass123");
            Assert.IsTrue(response.ErrorOccured, "should fail beacuse this isn't the users password (you may override the details when registering with existing email and other password)");
        }
        private void SuccessfulLogin_ExistingEmailAndOldRightPassword()
        {
            Response<User> response = Service.Login(User4.Email, User4.Password);
            Assert.IsFalse(response.ErrorOccured, $"the old user's (and correct password). \nYourError: {response.ErrorMessage}");
        }
        private void FailLogin_UserTryToLoginWhileLoggedIn()
        {
            Response<User> response = Service.Login(User4.Email, User4.Password);
            Assert.IsTrue(response.ErrorOccured, "should fail beacuse this user is already Logged in");
        }
        private void FailLogin_OtherUserTryToLoginWhileLoggedIn()
        {
            Response<User> response = Service.Login(User3.Email, User3.Password);
            Assert.IsTrue(response.ErrorOccured, "should fail beacuse this other user already Logged in");
            //Ensures the last Logged in user is still Logged in:
            Response<Column> responseColumn = Service.GetColumn(LoggedInUser.Email, 0);
            Assert.IsFalse(responseColumn.ErrorOccured, "while user x was Logged in, user y try to Login and unseccessfully," +
                $" user x should still be Logged in and return his column. \nYourError: {responseColumn.ErrorMessage}");
        }


        /*----------------------------------------------------------------*/

        private void TestTasksAndColumnsFunctions3()
        {
            // Please don't change the order of the calls
            SuccessfulAddTask_Exactly50CharsTitle();
            FailAddTask_100CharsTitle();
            SuccessfulAddTask_Exactly300CharsDescription();
            FailAddTask_400CharsDescription();
            FailAddTask_IllegalDueDate();
            SuccessfulAdvanceTask_FromBacklog();
            //Summary until now: 
            //user4 is Logged in, it has 1 task in the Backlog and 1 task in the InProgress. 
            //No tasks limitations.
            SuccessfulAddTask_SecondTask();
            FailLimitColumnTasks_BacklogTo1();
            AdvanceTask_BothTasksFromBacklog();
            //3 tasks in the InProgress column now
            SuccessfulTestUniqueTasksId();
            FailUpdateTaskDueDate_FromInProgress_IllegalDueDate();
            FailAdvanceTask_AdvancefromDoneColumn();

        }

        private void SuccessfulAddTask_Exactly50CharsTitle()
        {
            Response<Task> response = Service.AddTask(LoggedInUser.Email, new string('t', 50), "description", new DateTime(2020, 12, 12));
            Assert.IsFalse(response.ErrorOccured, $"Title can contain EXACTLY 50 chars, \nYourError: {response.ErrorMessage}");
        }

        private void FailAddTask_100CharsTitle()
        {
            Response<Task> response = Service.AddTask(LoggedInUser.Email, new string('t', 100), "description", new DateTime(2020, 9, 8));
            Assert.IsTrue(response.ErrorOccured, "add task should fail after long title given (above 50 chars)");
        }

        private void SuccessfulAddTask_Exactly300CharsDescription()
        {
            Response<Task> response = Service.AddTask(LoggedInUser.Email, "title", new string('d', 300), new DateTime(2020, 8, 9));
            Assert.IsFalse(response.ErrorOccured, $"Description can contain EXACTLY 300 chars, \nYourError: {response.ErrorMessage}");
        }

        private void FailAddTask_400CharsDescription()
        {
            Response<Task> response = Service.AddTask(LoggedInUser.Email, "title", new string('d', 400), new DateTime(2020, 7, 7));
            Assert.IsTrue(response.ErrorOccured, "add task should fail after long title given (above 50 chars)");
        }

        private void FailAddTask_IllegalDueDate()
        {
            Response<Task> response = Service.AddTask(LoggedInUser.Email, "title", "description", new DateTime(2020, 1, 1));
            Assert.IsTrue(response.ErrorOccured, "add task should fail after past due date given");
        }

        private void SuccessfulAdvanceTask_FromBacklog()
        {
            Response<Column> responseColumn = Service.GetColumn(LoggedInUser.Email, 0);
            Response response = Service.AdvanceTask(LoggedInUser.Email, 0, responseColumn.Value.Tasks.First().Id);
            Assert.IsFalse(response.ErrorOccured, $"successful AdvanceTask from backlog to inProgress column. \nYourError: {response.ErrorMessage}");
        }

        private void SuccessfulAddTask_SecondTask()
        {
            Response<Task> response = Service.AddTask(LoggedInUser.Email, "task2title", "description2", new DateTime(2020, 12, 12));
            Assert.IsFalse(response.ErrorOccured, $"normal Task Adding - no special reason to fail, \nYourError: {response.ErrorMessage}");
        }

        private void FailLimitColumnTasks_BacklogTo1()
        {
            Response response = Service.LimitColumnTasks(LoggedInUser.Email, 0, 1);
            Response<Column> responseColumn = Service.GetColumn(LoggedInUser.Email, 0);
            Assert.IsTrue(response.ErrorOccured, "limit backlog column with tasks in it");
            //Now checking that the trying of limitation cause no data loss:
            Assert.IsTrue(responseColumn.Value.Tasks.Count == 2, "the limitation attempt when (tasks.size > newLimit) should have fail, but it caused loss of 1 task (Expected 2 tasks)");
        }

        private void AdvanceTask_BothTasksFromBacklog()
        {
            Response<Column> response = Service.GetColumn(LoggedInUser.Email, 0);
            foreach (Task task in response.Value.Tasks)
                Service.AdvanceTask(LoggedInUser.Email, 0, task.Id);
        }

        private void SuccessfulTestUniqueTasksId()
        {
            Response<Column> response = Service.GetColumn(LoggedInUser.Email, 1);
            //var enumerator = response.Value.Tasks.GetEnumerator();
            int[] tasksId = new int[3];
            int counter = 0;
            foreach(Task task in response.Value.Tasks)
            {
                tasksId[counter] = task.Id;
                counter++;
            }

            //int task1 = enumerator.Current.Id; enumerator.MoveNext();
            //int task2 = enumerator.Current.Id; enumerator.MoveNext();
            //int task3 = enumerator.Current.Id;
            string errorMessage = "error in the id generator method, there are 2 tasks with the same id (id should be unique in the board and not in the column)";
            Assert.AreNotEqual(tasksId[0], tasksId[1], errorMessage);
            Assert.AreNotEqual(tasksId[1], tasksId[2], errorMessage);
            Assert.AreNotEqual(tasksId[0], tasksId[2], errorMessage);
        }

        private void FailUpdateTaskDueDate_FromInProgress_IllegalDueDate()
        {
            Response<Column> responseColumn = Service.GetColumn(LoggedInUser.Email, 1);
            Response response = Service.UpdateTaskDueDate(LoggedInUser.Email, 1, responseColumn.Value.Tasks.First().Id, new DateTime(2019, 7, 3));
            Assert.IsTrue(response.ErrorOccured, "update task from in progress to with new Illegal DueDate (past due date). " +
                "\nThere is NO more test for other updates - if this test fails, make sure to check validation of UpdateTitle and UpdateDescription.");
        }

        private void FailAdvanceTask_AdvancefromDoneColumn()
        {
            Response<Column> responseColumn = Service.GetColumn(LoggedInUser.Email, 1);
            int taskSrcId = responseColumn.Value.Tasks.First().Id;
            Service.AdvanceTask(LoggedInUser.Email, 1, taskSrcId); //move task to done

            responseColumn = Service.GetColumn(LoggedInUser.Email, 2);
            int taskDestId = responseColumn.Value.Tasks.FirstOrDefault(task => task.Id == taskSrcId).Id;
            Assert.IsTrue(taskDestId == taskSrcId, "task has not advanced properly OR task id has changed in the advancing");

            Response response = Service.AdvanceTask(LoggedInUser.Email, 2, taskDestId);
            Assert.IsTrue(response.ErrorOccured, "tasks cannot be advanced from the done column");
        }




        /**********************************************************************************************************************
                                            Test 4 : ISIS Terorist
          ---------------------------------------------------------------------------------------------------
                   This test tests mostly null, whitespaces, and other invalid input - fun, fun & fun

         IMPORTANT: You may pass this tests easily - pay attention that your service is probably catching ALL the exceptions.
         The test CANT identify whether the caught exception is thrown form YOUR code or from the SYSTEM.
         I suggest you to debug this test and check manually this issue 
         OR 
         check your LOG FILE to find out
         OR
         Watch the last minutes of Achia (!?) Office hours (19.4) - When he explained "MyException"
         *********************************************************************************************************************/





        [TestMethod]
        public void Test4()
        {
            Service = new Service();
            //Service.LoadData();
            User1 = new TestUser("user1@gmail.com", "MyPass123", "myCoolName");
            User2 = new TestUser("user2@gmail.com", "MyPass321", "user4ever");
            User3 = new TestUser("user3@gmail.com", "Abc1", "nickname3");
            User4 = new TestUser("user4@gmail.com", "AAAAAbbbbbccccc12345", "nickname4");

            // Please don't change the order of the calls
            TestUsersFunctions();
            TestTasksAndColumnsFunctions();
        }

        /*----------------------------------------------------------------*/

        private void TestUsersFunctions()
        {
            // Please don't change the order of the calls
            //FailRegister_OperationBeforeLoadData();
            Service.LoadData();
            FailRegister_NullOrWhiteSpaceEmail();
            FailRegister_NullOrWhiteSpacePassword();
            FailRegister_NullOrWhiteSpaceNickname();
            FailLogin_NullEmail();
            FailGetBoard_NullEmail(); //no user is connected
            SuccessfulLogin_User4();
            LoggedInUser = User4;
        }

        private void FailRegister_OperationBeforeLoadData()
        {
            Response response = Service.Register("mail@gmail.com", "Pass555", "Pishoto");
            Assert.IsTrue(response.ErrorOccured, "LoadData function has not called yet - user can't make any operation");
        }

        private void FailRegister_NullOrWhiteSpaceEmail()
        {
            Response response = Service.Register(null, "Password123", "Dror");
            Assert.IsTrue(response.ErrorOccured, "null email is illegal Argument");
            response = Service.Register("   ", "Password123", "Dror");
            Assert.IsTrue(response.ErrorOccured, "empty or spaces email is illegal Argument");
        }

        private void FailRegister_NullOrWhiteSpacePassword()
        {
            Response response = Service.Register("usermail@gmail.com", null, "nick");
            Assert.IsTrue(response.ErrorOccured, "null password is illegal Argument");
        }

        private void FailRegister_NullOrWhiteSpaceNickname()
        {
            Response response = Service.Register("usermail@gmail.com", "Password123", null);
            Assert.IsTrue(response.ErrorOccured, "null nickname is illegal Argument");
            response = Service.Register("usermail", "Password123", "    ");
            Assert.IsTrue(response.ErrorOccured, "empty or spaces nickname is illegal Argument");
        }

        private void FailLogin_NullEmail()
        {
            Response<User> response = Service.Login(null, "Password123");
            Assert.IsTrue(response.ErrorOccured, "null email is illegal Argument");
        }

        private void FailGetBoard_NullEmail()
        {
            Response<Board> response = Service.GetBoard(null);
            Assert.IsTrue(response.ErrorOccured, "null email is illegal Argument");
        }

        private void SuccessfulLogin_User4()
        {
            Response<User> response = Service.Login(User4.Email, User4.Password);
            Assert.IsFalse(response.ErrorOccured, $"legal Login. \nYourError: {response.ErrorMessage}");
        }

        /*----------------------------------------------------------------*/

        private void TestTasksAndColumnsFunctions()
        {
            // Please don't change the order of the calls
            FailLimitColumnTasks_WrongColumnOrdinal();
            //FailAddTask_NullTaskDetails();
            SuccessfulAddTask();
            FailAdvanceTask_WrongColumnOrdinal();
            FailGetColumn_WrongName();
        }

        private void FailLimitColumnTasks_WrongColumnOrdinal()
        {
            Response response = Service.GetColumn(LoggedInUser.Email, 3);
            Assert.IsTrue(response.ErrorOccured, "column ordinal 3 is not exist");
            response = Service.GetColumn(LoggedInUser.Email, -1);
            Assert.IsTrue(response.ErrorOccured, "column ordinal -1 is not exist");
        }

        private void FailAddTask_NullTaskDetails()
        {
            Response<Task> response = Service.AddTask(LoggedInUser.Email, null, "Desc", new DateTime(2022, 2, 2));
            Assert.IsTrue(response.ErrorOccured, "can't add task with null title");
            response = Service.AddTask(LoggedInUser.Email, "Title", null, new DateTime(2022, 2, 2));
            Assert.IsTrue(response.ErrorOccured, "can't add task with null Description");
        }

        private void SuccessfulAddTask()
        {
            Response<Task> response = Service.AddTask(LoggedInUser.Email, "tasktitle", "description", new DateTime(2020, 12, 12));
            Assert.IsFalse(response.ErrorOccured, $"normal Task Adding - no special reason to fail, \nYourError: {response.ErrorMessage}");
        }

        private void FailAdvanceTask_WrongColumnOrdinal()
        {
            Response response = Service.AdvanceTask(LoggedInUser.Email, 3, 1);
            Assert.IsTrue(response.ErrorOccured, "can't advance task from non existing column");
        }

        private void FailGetColumn_WrongName()
        {
            Response<Column> response = Service.GetColumn(LoggedInUser.Email, "NONAME");
            Assert.IsTrue(response.ErrorOccured, "can't get column with not existing name");
        }
    }
}
