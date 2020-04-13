using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    namespace BoardPackage
    {
        class Task
        {

            private string title;
            private string description;
            private DateTime creationDate;
            private DateTime dueDate;

            public Task(string title, string description, DateTime dueDate)
            {
                if (!ValidateTitle(title) | !ValidateDescription(description) | !ValidateDueDate(dueDate))
                    throw new Exception("One or more of the parameters illegal");
                this.title = title;
                this.description = description;
                this.creationDate = DateTime.Now;
                this.dueDate = dueDate;
            }

            public void UpdateTaskDueDate(DateTime dueDate)
            {
                if (!ValidateDueDate(dueDate))
                    throw new Exception("The dueDate is Illegal(already passed)");
                this.dueDate = dueDate;
            }

            public void UpdateTaskTitle(string title)
            {
                if (!ValidateTitle(title))
                    throw new Exception("Illegal title(over 50 characters)");
                this.title = title;
            }

            public void UpdateTaskDescription(string description)
            {
                if (!ValidateDescription(description))
                    throw new Exception("Description not fit(over than 300 character)");
                this.description = description;
                      
            }

            private bool ValidateTitle(string title)
            {
                return title.Length <= 50;
            }

            private bool ValidateDescription(string newDesc)
            {
                return newDesc.Length <= 300;
            }

            private bool ValidateDueDate(DateTime newDue)
            {
                bool res = newDue.Year >= DateTime.Now.Year;
                res = res & newDue.Month >= DateTime.Now.Month;
                res = res & newDue.Day >= DateTime.Now.Day;
                res = res & newDue.Hour >= DateTime.Now.Hour;
                res = res & newDue.Minute >= DateTime.Now.Minute;
                res = res & newDue.Second >= DateTime.Now.Second;
                return res;
            }
        }
    }
}
