using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    namespace UserPackage
    {
        class User
        {
            private String email;
            private string password;
            private string nickname;
            private myBoard Board;


            public User(string email, string password, string nickname)
            {
                this.email = email;
                this.password = password;
                this.nickname = nickname;
                myBoard = new BoardPackage.Board();
            }

            public Boolean ValidatePassword(string password)
            {
                return this.password == password;
            }

            public string GetEmail()
            {
                return email;
            }

            public Board GetBoard()
            {
                return Board;
            }
        }
    }
}
