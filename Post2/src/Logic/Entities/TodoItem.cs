using Logic.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Entities
{
    public class TodoItem : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }

        public DateTime Created { get; set; }
        public DateTime? CompleteDate { get; set; }

    }
}
