using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct User
    {
        public readonly string Email;
        public readonly string Nickname;
        private readonly BusinessLayer.BoardPackage.Board board;
        internal User(string email, string nickname)
        {
            this.Email = email;
            this.Nickname = nickname;
            this.board = null;
        }
        // You can add code here

        internal User(string email,string nickname,BusinessLayer.BoardPackage.Board board)
        {
            this.Email = email;
            this.Nickname = nickname;
            this.board = board;
        }

        internal BusinessLayer.BoardPackage.Board GetBoard()
        {
            return board;
        }
    }
}
