using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Task = IntroSE.Kanban.Backend.ServiceLayer.Task;

namespace tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Service s = new Service();
            s.LoadData();

            //s.DeleteData();


            //s.Register("fsdfdsf@gmail.stfu", "dsD2dkjgiusdci;asdgiaxfhisdby", "stfu");

            //s.DeleteData();


            //s.Register("yoad@gmail.com", "123dfds5sdS", "fdf");
            //s.Register("avishay@gmail.com", "dfsf55D", "ts", "harel@gmail.com");
            //s.Register("harel@gmail.com", "12345sdS", "nnick");

            //s.Register("fdfdddsf@sdsg", "sdfsdDdFFF985", "helo");

            //s.Login("harel@gmail.com", "12345sdS");

            //s.Register("ksgjhkdsfh@gmail.com", "sshfgdghfAS213", "tedfhgdghght", "hello@gmail.com");

            //s.Register("hello@gmail.com", "123456Aa", "hello");
            s.Login("hello@gmail.com", "123456Aa");


            //s.AssignTask("hello@gmail.com", 2, 4, "ksgjhkdsfh@gmail.com");
            //s.ChangeColumnName("hello@gmail.com", 4, "shmoolik");
            //s.UpdateTaskTitle("fgdf@gmail.com", 3, 0, " ");
            



            s.Logout("hello@gmail.com");
            //s.AssignTask("harel@gmail.com", 0, 1, "avishay@gmail.com");
            //s.AddTask("harel@gmail.com", "new", "newnew", new DateTime(2020, 8, 8));
            //s.DeleteTask("harel@gmail.com", 0, 1);
            //s.DeleteTask("harel@gmail.com", 0, 0);
            //s.ChangeColumnName("harel@gmail.com", 1, "donedone");
            //s.MoveColumnLeft("harel@gmail.com", 1);
            //s.RemoveColumn("harel@gmail.com", 1);
            //s.RemoveColumn("harel@gmail.com", 1);
            //s.AdvanceTask("harel@gmail.com", 0, 0);
            //s.AddTask("harel@gmail.com", "title", "desc", new DateTime(2020, 6, 20));
            //s.AssignTask("harel@gmail.com", 0, 0, "csdvdbf@gmail.com");
            //s.Register("sHJDF@gmail.com", "dsfFFf6", "dfssd");
            //s.Login("sHJDF@gmail.com", "dsfFFf6");

            //s.AddColumn("sHJDF@gmail.com", 4, "lastlast");
            //s.AddColumn("sHJDF@gmail.com", 0, "firstfirst");
            //s.AddColumn("sHJDF@gmail.com", 2, "mid");

            //s.AddTask("sHJDF@gmail.com", "hellohello", "descdesc", new DateTime(2020, 7, 10));
            //s.UpdateTaskDescription("harel@gmail.com", 0, 0, "fdsgdf");

            //s.RemoveColumn("sHJDF@gmail.com", 0);
            //s.AddColumn("sHJDF@gmail.com", 2, "lastlast");


            //s.RemoveColumn("sHJDF@gmail.com", 0);

            /*
            s.GetBoard("sHJDF@gmail.com").Value.ColumnsNames.ToList<string>().ForEach((delegate (String name)
            {
                IReadOnlyCollection<Task> i = s.GetColumn("sHJDF@gmail.com", name).Value.Tasks;
                foreach (Task t in i)
                {
                    Console.WriteLine(t.Id+" "+t.Title+" "+t.Description+" "+t.CreationTime+" "+t.DueDate);
                }
            }));
            */
            //s.RemoveColumn("sHJDF@gmail.com", 3);
            //Response<Column> response = s.AddColumn("shJdf@gmail.com", 2, "test");
            //s.AddTask("sHJDF@gmail.com", "toUpdate", "desc", new DateTime(2020, 7, 6));
            //s.AdvanceTask("sHJDF@gmail.com", 1, 0);
            //s.MoveColumnLeft("sHJDf@gmail.com", 3);
            //s.UpdateTaskTitle("sHJDF@gmail.com", 2, 0, "test");
            //s.AddColumn("sHJDF@gmail.com", 0, "first");
            //s.AddColumn("sHJDF@gmail.com", 2, "last");
            //s.LimitColumnTasks("sHJDF@gmail.com", 2, 0);



            //s.Logout("sHJDF@gmail.com");
            //s.AddColumn("harel@gmail.com", 5, "test");

            //s.RemoveColumn("harel@gmail.com", 0);
            //s.RemoveColumn("harel@gmail.com", 0);
            //s.RemoveColumn("harel@gmail.com", 0);

            //s.AddColumn("harel@gmail.com", 0, "hello");


            //s.Logout("harel@gmail.com");
            //s.Login("harel@gmail.com", "12345sdS");
            //s.AddColumn("harel@gmail.com", 1, "hello");
            //s.MoveColumnLeft("harel@gmail.com", 3);






            //s.AddTask("harel@gmail.com", "first", "desc", new DateTime(2020, 6, 6));
            //s.AdvanceTask("harel@gmail.com", 0, 2);
            //s.AdvanceTask("harel@gmail.com", 1, 0);
            //
            //s.RemoveColumn("harel@gmail.com", 0);

            //s.RemoveColumn("harel@gmail.com", 0);
            //s.RemoveColumn("harel@gmail.com", 0);

            //s.AddColumn("harel@gmail.com", 1, "stfu");

            //s.UpdateTaskDescription("harel@gmail.com", 0, 1, "hello dumbass");
            //s.UpdateTaskTitle("harel@gmail.com", 0, 1, "dumbbutt");
            //s.UpdateTaskDueDate("harel@gmail.com", 0, 1, new DateTime(2020, 6 , 7));

            //s.LimitColumnTasks("harel@gmail.com", 0, 4);



            //s.Logout("harel@gmail.com");
            //s.AddColumn("harel@gmail.com", 2, "second");
            //s.Logout("harel@gmail.com");

            //s.Register("haRelAfriat@gmaiL.COm", "123456aA", "Harel");


            //s.Register("ccc c@mail.com", "132xfdF", "harel");
            //s.Login("harelafriat@gmail.com", "123456aA");

            //Console.WriteLine(test("fsf=!$%^#^&sdf*@gmail.com"));
            //s.AddTask("harel@gmail.com", "second", null, new DateTime(2020,6,6));
            //s.MoveColumnLeft("harel@gmail.com", 2);
            //s.RemoveColumn("harel@gmail.com", 2);
            //s.AdvanceTask("harelafriat@gmail.com", 0, 0);

            //Console.WriteLine(CheckProperPassToRegister(null));


            //Console.WriteLine(s.LimitColumnTasks("harelafriat@gmail.com", 1, 0).ErrorMessage);
            /*
            s.AddTask("harelafriat@gmail.com", "0", "", new DateTime(2020, 6, 6));
            s.AddTask("harelafriat@gmail.com", "1", null, new DateTime(2020, 6, 6));
            s.AddTask("harelafriat@gmail.com", "2", "desc", new DateTime(2020, 6, 6));
            s.AddTask("harelafriat@gmail.com", "3", "desc", new DateTime(2020, 6, 6));
            s.AddTask("harelafriat@gmail.com", "4", "desc", new DateTime(2020, 6, 6));
            

            s.AdvanceTask("harelafriat@gmail.com", 0, 2);
            */

            //s.AddTask("harelAfrIat@gmail.com", "third", "" , new DateTime(2020, 6, 20));
            //s.AdvanceTask("HaRelafriat@gmail.com", 1, 0);

            //s.LimitColumnTasks("HareLafriat@gmail.com", 2, 5);

            //s.UpdateTaskDescription("harelAfriat@gmail.com", 0, 0, null);

            //s.LimitColumnTasks("HareLAfriat@gmail.cOM", 1, 0);
            //s.AdvanceTask("HARELAFRIAT@GMAIL.COM", 0, 0);

            //s.Logout("haRElafrIat@gmail.com");

            Console.ReadKey();
        }

        public static bool test(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                throw e;
            }
            catch (ArgumentException e)
            {
                throw e;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\\+/=\?\^`\{\}\|~\w]))(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z][0-9a-z]\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private static bool IsLegalEmailAdress(string Email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(Email);
                if (Email.IndexOf('@') == -1)
                    throw new Exception("Ilegal email, the email must contains @");
                int index = Email.IndexOf('@');
                int counter = 0;

                if (Email.Substring(index + 1).Contains("@"))
                    throw new Exception("Ilegal email, the email contains more than one @");

                for (int i = index + 1; i < Email.Length; i++)
                {
                    if (Email[i] != '.')
                        counter++;
                    else if (counter < 2)
                        throw new Exception("Ilegal email, every generic top level must contains 2 or more characters");
                    else
                        counter = 0;
                }

                if (counter < 2)
                    throw new Exception("Ilegal email, every generic top level must contains 2 or more characters");

                return addr.Address == Email;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckProperPassToRegister(string password)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(password))
                    throw new Exception("test");
                if (password.Length > 20 | password.Length < 4)
                    throw new Exception("This password is not between 4-20 characters");
                bool isExistSmallChar = false;
                bool isExistCapitalLetter = false;
                bool isExistNumber = false;
                for (int index = 0; index < password.Length & (!isExistCapitalLetter | !isExistSmallChar | !isExistNumber); index++)
                {
                    for (char i = 'a'; i <= 'z' & !isExistSmallChar; i++)
                    {
                        if (password[index] == i)
                        {
                            isExistSmallChar = true;
                        }
                    }

                    for (char i = 'A'; i <= 'Z' & !isExistCapitalLetter; i++)
                    {
                        if (password[index] == i)
                        {
                            isExistCapitalLetter = true;
                        }
                    }

                    for (char i = '0'; i <= '9' & !isExistNumber; i++)
                    {
                        if (password[index] == i)
                        {
                            isExistNumber = true;
                        }
                    }

                }

                if (isExistNumber == false | isExistCapitalLetter == false | isExistSmallChar == false)
                    throw new Exception("The password does not contains at least one small character, one Capital letter and one digit");
                
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
