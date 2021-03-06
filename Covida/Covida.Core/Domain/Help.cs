﻿using Covida.Core.Definition;
using Covida.Core.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Covida.Core.Domain
{
    public class Help : IDomain
    {
        public Guid Id { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string CancelledReason { get; set; }
        public HelpStatus HelpStatus { get; set; }
        public User Author { get; set; }
        public int AuthorId { get; set; }
        public User Volunteer { get; set; }
        public int? VolunteerId { get; set; }
        public ICollection<HelpHasCategory> HelpHasCategories { get; set; }
        public ICollection<HelpItem> HelpItems { get; set; }
        public ICollection<Message> Messages { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
