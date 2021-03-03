using Domain.Common;

namespace Domain.Entities
{
    public class Entity : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Age { get; set; }
        public double Factor { get; set; }
    }
}