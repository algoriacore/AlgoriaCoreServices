using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    public partial class ToDoActEvaluator : Entity<long>
    {
        public long? ToDoActivity { get; set; }
        public long User { get; set; }

        public virtual ToDoActivity ToDoActivityNavigation { get; set; }
        public virtual User UserNavigation { get; set; }
    }
}
