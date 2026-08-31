using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolService.Application.Common.Mediator
{
    public class ValidationHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _innerHandler;
        public ValidationHandler(IRequestHandler<TRequest, TResponse> requestHandler)
        {
            _innerHandler = requestHandler;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            if (request is IValidatableRequest validatable)
            {
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(validatable.GetValidationTarget(), new ValidationContext(validatable.GetValidationTarget()), validationResults, validateAllProperties: true))
                {
                    throw new ArgumentException(string.Join("; ", validationResults.Select(r => r.ErrorMessage)));
                }
            }
            return await _innerHandler.Handle(request, cancellationToken);
        }
    }
}
