using System;

namespace KanbanTesting
{
    internal struct TestTask
    {
        internal string Title;
        internal string Description;
        internal DateTime DueDate;
        internal TestTask(string title, string description, DateTime dueDate)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
        }
    }
}