using System;

namespace Domain.Common
{
    public class AuditableEntity
    {
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
    }
}