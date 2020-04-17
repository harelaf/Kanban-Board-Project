using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class RegisteredEmail : DalObject<RegisteredEmail>
    {
        public List<string> Emails;
        public DalController DCtrl;
        public const string FILE_NAME = "RegisteredEmails";
        public RegisteredEmail(DalController DCtrl,List<string> Emails)
        {
            this.DCtrl = DCtrl;
            this.Emails = Emails;
        }

        public RegisteredEmail()
        {
            Emails = new List<string>();
            DCtrl = new DalController();
        }

        public override string FromJson(string json)
        {
            return DCtrl.Read(json);
        }

        public override RegisteredEmail Import()
        {
            return JsonSerializer.Deserialize<RegisteredEmail>(FromJson(FILE_NAME));
        }

        public override void Save()
        {
            DCtrl.Write(FILE_NAME, ToJson());
        }
    }
}
