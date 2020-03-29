using Covida.Core.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Covida.Infrastructure.Definitions
{
    public interface IActorAwareRequest<T> : IRequest<T>
    {
        User Actor { get; set; }
    }
}
