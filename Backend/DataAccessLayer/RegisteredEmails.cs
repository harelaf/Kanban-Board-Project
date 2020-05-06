using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class RegisteredEmails
    {
        public List<string> Emails { get; set; }
        public DalController DCtrl { get; set; }
        private const string FILE_NAME = "RegisteredEmails";

        public RegisteredEmails(DalController DCtrl,List<string> Emails)
        {
            this.DCtrl = DCtrl;
            this.Emails = Emails;
        }

        public RegisteredEmails()
        {
            Emails = new List<string>();
            DCtrl = new DalController();
        }

        public RegisteredEmails Import()
        {
            throw new NotImplementedException();
        }

    }
}
