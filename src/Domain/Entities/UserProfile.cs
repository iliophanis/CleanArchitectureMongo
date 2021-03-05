using Domain.Common;

namespace Domain.Entities
{
    public class UserProfile : AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Bio { get; set; }
    }
}