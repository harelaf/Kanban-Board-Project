using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Task
    {
        public readonly int Id;
        public readonly DateTime CreationTime;
        public readonly string Title;
        public readonly string Description;
        internal Task(int id, DateTime creationTime, string title, string description)
        {
            this.Id = id;
            this.CreationTime = creationTime;
            this.Title = title;
            this.Description = description;
        }
        // You can add code here
    }
}
