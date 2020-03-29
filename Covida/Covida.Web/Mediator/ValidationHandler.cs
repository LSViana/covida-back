using Covida.Infrastructure.Exceptions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Covida.Web.Mediator
{
    public class ValidationHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationHandler(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var failedValidations = validators
                .Select(x => x.Validate(request))
                .Where(x => !x.IsValid)
                .ToArray();

            if (failedValidations.Any())
            {
                var errors = failedValidations.SelectMany(x => x.Errors.Select(y => new ValidationResult()
                {
                    Error = y.ErrorMessage,
                    Property = y.PropertyName,
                    Value = y.AttemptedValue,
                }))
                .Distinct()
                .ToArray();
                throw new HttpException(HttpStatusCode.BadRequest, errors);
            }

            return await next();
        }
    }

    public struct ValidationResult
    {
        public string Property { get; set; }
        public string Error { get; set; }
        public object Value { get; set; }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(ValidationResult left, ValidationResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ValidationResult left, ValidationResult right)
        {
            return !(left == right);
        }
    }
}
