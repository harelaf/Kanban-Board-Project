using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class RegisteredEmail : DalObject<RegisteredEmail>
    {
        public List<string> Emails;
        public DalController DCtrl;
        public RegisteredEmail(DalController DCtrl,List<string> Emails)
        {
            this.DCtrl = DCtrl;
            this.Emails = Emails;
        }

        public override string FromJson(string json)
        {
            throw new NotImplementedException();
        }

        public override RegisteredEmail Import()
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }
    }
}
