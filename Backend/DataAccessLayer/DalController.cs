using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DalController
    {
        public string Read(string filename)
        {
            return File.ReadAllText(filename + ".json");
        }

        public void Write(string filename, string content)
        {
            File.WriteAllText(filename + ".json", content);
        }
    }
}
