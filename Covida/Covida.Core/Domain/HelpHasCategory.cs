using Covida.Core.Definition;
using System;
using System.Collections.Generic;
using System.Text;

namespace Covida.Core.Domain
{
    public class HelpHasCategory : IDomain
    {
        public Guid HelpId { get; set; }
        public Guid HelpCategoryId { get; set; }
        public Help Help { get; set; }
        public HelpCategory HelpCategory { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
