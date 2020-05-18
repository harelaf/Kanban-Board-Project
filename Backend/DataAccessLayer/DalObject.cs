using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    abstract class DalObject<T> where T : DalObject<T>
    {
        public abstract void Save();

        public abstract T Import();

        public abstract void Delete();
    }
}
