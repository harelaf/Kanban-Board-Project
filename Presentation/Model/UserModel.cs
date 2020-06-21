using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Presentation.Model
{
    class UserModel : NotifiableModelObject
    {
        public readonly string Email;
        public readonly string Nickname;

        public UserModel(BackendController Controller, string Email, string Nickname) : base(Controller)
        {
            this.Email = Email;
            this.Nickname = Nickname;
        }
    }
}
