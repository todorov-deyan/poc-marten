using Marten;

using PocMarten.Api.Common.Repository;

namespace PocMarten.Api.Aggregates.Order.Repository
{
    public class OrderRepository : MartenRepository<Models.OrderModel>
    {
        public OrderRepository(IDocumentSession documentSession) 
            : base(documentSession)
        {
        }
    }
}
