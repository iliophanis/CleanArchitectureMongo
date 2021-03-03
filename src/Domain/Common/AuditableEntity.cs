using System;

namespace Domain.Common
{
    public class AuditableEntity
    {
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}