using System;
using NUnit.Framework;
using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;

namespace KanbanTests
{
    public class UnitTest1
    {
        UserController Controller;
        
        [SetUp]
        public void SetUp()
        {
            Controller = new UserController();
        }

        [TestCase("123456Aa")]
        [TestCase("123Aa")]
        [TestCase("01234567890123456789012Aa")]
        [TestCase("AAAAAAAAaaaaa5")]
        [TestCase("6546a5sdfffDDdd")]
        [TestCase("ADd5d")]
        public void Test_Vaild_Password(string Password)
        {
            //Assign
            bool ans;
            //Act
            try
            {
                ans = Controller.CheckProperPassToRegister(Password);
            }
            catch
            {
                ans = false;
            }
            //Assert
            Assert.AreEqual(true, ans, "The passowrd " + Password + " is valid, expected to get true, but got false");
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("aaaaa")]
        [TestCase("AAAAA")]
        [TestCase("11111")]
        [TestCase("Aa1")]
        [TestCase("aaaAA")]
        [TestCase("AAaa11aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        [TestCase("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA11111aaaaaa")]
        [TestCase("123AA")]
        [TestCase("a")]
        [TestCase("1")]
        [TestCase("A")]
        public void Test_Invaild_Password(string Password)
        {
            //Assign
            bool ans;
            //Act
            try
            {
                ans = Controller.CheckProperPassToRegister(Password);
            }
            catch
            {
                ans = false;
            }
            //Assert
            Assert.AreEqual(false, ans, "The password " + Password + "is invaild, expected to get false but got true");
        }

        [TestCase("hello@gmail.com")]
        [TestCase("o@gmail.com")]
        [TestCase("hello@post.bgu.ac.il")]
        [TestCase("helsdfsdflo@gmail.com")]
        [TestCase("aa@aa.com")]
        [TestCase("yoadoh1@gmail.com")]
        public void Test_Vaild_Email(string Email)
        {
            //Assign
            bool ans;
            //Act
            try
            {
                ans = Controller.IsLegalEmailAdress(Email);
            }
            catch
            {
                ans = false;
            }
            //Assert
            Assert.AreEqual(true, ans, "The email: " + Email + "is vaild, expected to get true but got false");
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("a")]
        [TestCase("@")]
        [TestCase(".")]
        [TestCase(".com")]
        [TestCase("a@com")]
        [TestCase("a@.com")]
        [TestCase("a@a.com")]
        [TestCase("@gmail..")]
        [TestCase("hello.com")]
        [TestCase("@@")]
        [TestCase("harel@g.g.com")]
        [TestCase("harel@gdd.gdd.c")]
        public void Test_Invaild_Email(string Email)
        {
            //Assign
            bool ans;
            //Act
            try
            {
                ans = Controller.IsLegalEmailAdress(Email);
            }
            catch
            {
                ans = false;
            }
            //Assert
            Assert.AreEqual(false, ans, "The email: " + Email + "is invaild, expected to get false but got true");
        }

        [TestCase("hello@gmail.com", "123456Aa", "hello")]
        [TestCase("harel@gmail.com", "1234567891011Aa", "harel")]
        public void Test_Vaild_Register(string Email, string Password, string Nickname)
        {
            //Assign
            //Act
            Controller.Register(Email, Password, Nickname);
            //Assert
            Assert.AreEqual(1, Controller.UserList.Count, "Register didn't add the new user to the user list, expected to get 1");
        }

        [TestCase("@gmail.com", "123456Aa", "hello")]
        [TestCase(null, null, null)]
        [TestCase("email@email.com", null, null)]
        [TestCase(null, "pass45AAA", null)]
        [TestCase(null, null, "nickNick")]
        [TestCase("hello@gmail.com", "123456Aa", null)]
        [TestCase("hello@gmail.com", "123456Aa", "")]
        public void Test_Invaild_Register(string Email, string Password, string Nickname)
        {
            //Assign
            int oldAmount = Controller.UserList.Count;
            //Act
            try
            {
                Controller.Register(Email, Password, Nickname);
            }
            catch
            {

            }
            //Assert
            Assert.AreEqual(0, Controller.UserList.Count - oldAmount, "Register added the new user to the user list but it was invaild, expected to get 0");
        }

        [Test]
        public void Test_Register_User_Exists()
        {
            //Assign
            string Email = "testUser@gmail.com";
            string Password = "123456Aa";
            string Nickname = "testUser";
            Controller.Register(Email, Password, Nickname);
            int oldAmount = Controller.UserList.Count;
            //Act
            try
            {
                Controller.Register(Email, Password, Nickname);
            }
            catch
            {

            }
            //Assert
            Assert.AreEqual(0, Controller.UserList.Count - oldAmount, "Register added the new user to the user list but a user with the same email had already registered, expected to get 0");
        }
    }
}
