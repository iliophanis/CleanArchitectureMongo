using System.Collections.Generic;

namespace Application.Common.Models
{
    public class Envelope<T>
    {
        public List<T> Items { get; set; }
        public long Count { get; set; }
    }
}