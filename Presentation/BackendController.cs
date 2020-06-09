using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Presentation
{
    class BackendController
    {
        private Service MyService { get; set; }

        public BackendController()
        {
            this.MyService = new Service();
            MyService.LoadData();
        }

        public void Register(string Email, string Nickname, string Password, string HostEmail)
        {
            Response response = (HostEmail == "" ? MyService.Register(Email, Password, Nickname) : MyService.Register(Email, Password, Nickname, HostEmail));
            if (response.ErrorOccured) throw new Exception(response.ErrorMessage);
        }
    }
}
