using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class User : DalObject<User>
    {
        public string email { get; set; }
        public string password { get; set; }
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
            this.email = email.ToLower();
            this.password = password;
            this.nickname = nickname;
            this.myBoard = myBoard;
            dalController = new DalController();
        }

        public User(string email)
        {
            this.email = email.ToLower();
            password = null;
            nickname = null;
            myBoard = null;
            dalController = new DalController();
        }

        public override void Save()
        {
            dalController.Write(ToFilename(), ToJson());
        }

        public override User Import()
        {
            return JsonSerializer.Deserialize<User>(FromJson(ToFilename()));
        }

        public override string FromJson(string json)
        {
            return dalController.Read(json);
        }

        private string ToFilename()
        {
            return email.Replace('@', '.');
        }

        public override string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
