using Covida.Core.Definition;
using System;
using System.Collections.Generic;

namespace Covida.Core.Domain
{
    public class User : IDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public bool IsVolunteer { get; set; }
        public ICollection<Help> Helps { get; set; }
        public ICollection<Help> Answers { get; set; }
        public ICollection<Message> Messages { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
