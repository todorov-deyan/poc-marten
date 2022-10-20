using MediatR;
using Microsoft.AspNetCore.Mvc;
using PocMarten.Api.Aggregates.BicoinExchangeRate.Models;
using PocMarten.Api.Aggregates.BicoinExchangeRate.Repository;
using PocMarten.Api.Common.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocMarten.Api.Aggregates.BicoinExchangeRate.Queries
{
    public record GetExchangeRateByIdQuery(Guid id) : IRequest<ExchangeRateDetails>;

    public class GetExchangeRateByIdQueryHandler : IRequestHandler<GetExchangeRateByIdQuery, ExchangeRateDetails>
    {
        private readonly ExchangeRateRepository _repository;

        public GetExchangeRateByIdQueryHandler(ExchangeRateRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExchangeRateDetails> Handle(GetExchangeRateByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.Find(request.id, cancellationToken);
 
            return result;
        }
    }
}
