﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class RegisteredEmails : DalObject<RegisteredEmails>
    {
        public List<string> Emails;
        public DalController DCtrl;
        public const string FILE_NAME = "RegisteredEmails";
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

        public override string FromJson(string json)
        {
            return DCtrl.Read(json);
        }

        public override RegisteredEmails Import()
        {
            return JsonSerializer.Deserialize<RegisteredEmails>(FromJson(FILE_NAME));
        }

        public override void Save()
        {
            DCtrl.Write(FILE_NAME, ToJson());
        }
    }
}
