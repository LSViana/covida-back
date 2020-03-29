using Covida.Core.Definition;
using Covida.Core.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Covida.Core.Domain
{
    public class Message : IDomain
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public MessageStatus MessageStatus { get; set; }
        public Guid HelpId { get; set; }
        public int UserId { get; set; }
        public Help Help { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
