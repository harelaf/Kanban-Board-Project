using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Presentation.Model
{
    class UserModel
    {
        private string Email;
        private string Nickname;
        private BoardModel Board;

        public UserModel(string Email, string Nickname , BoardModel Board)
        {
            this.Email = Email;
            this.Nickname = Nickname;
            this.Board = Board;
        }
    }
}
