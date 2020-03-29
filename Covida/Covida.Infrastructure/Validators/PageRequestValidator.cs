using Covida.Infrastructure.Definitions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Covida.Infrastructure.Validators
{
    public class PageRequestValidator<T> : AbstractValidator<PageRequest<T>> where T : new()
    {
        public PageRequestValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        }
    }
}
