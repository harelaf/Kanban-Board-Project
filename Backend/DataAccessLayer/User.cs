using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class User : DalObject<User>
    {
        public string email { get; set; }
        private string password { get; set; }
        public string nickname { get; set; }
        public Board myBoard { get; set; }
        public DalController dalController;

        public User()
        {
            email = null;
            password = null;
            nickname = null;
            myBoard = null;
            dalController = new DalController();
        }

        public User(string email, string password, string nickname, Board myBoard)
        {
            this.email = email;
            this.password = password;
            this.nickname = nickname;
            this.myBoard = myBoard;
            dalController = new DalController();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override User Import()
        {
            throw new NotImplementedException();
        }

        public override string FromJson(string json)
        {
            throw new NotImplementedException();
        }
    }
}
