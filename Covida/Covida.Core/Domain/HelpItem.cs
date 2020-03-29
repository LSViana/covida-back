using Covida.Core.Definition;
using System;
using System.Collections.Generic;
using System.Text;

namespace Covida.Core.Domain
{
    public class HelpItem : IDomain
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public uint Amount { get; set; }
        public bool Complete { get; set; }
        public int HelpId { get; set; }
        public Help Help { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
