using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    interface IPersistedObject<T> where T : DalObject<T>
    {
        T ToDalObject(string Email, int colOrdinal);
    }
}
