using Covida.Core.Definition;
using System;
using System.Collections.Generic;
using System.Text;

namespace Covida.Core.Domain
{
    public class HelpCategory : IDomain
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<HelpHasCategory> CategoryHasHelps { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
