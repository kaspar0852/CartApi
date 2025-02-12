using Cart.Api.Entities;
using Cart.Api.EntityFramework;
using Contracts;
using MassTransit;

namespace Cart.Api.EventHandlers;

public class OrderCreatedEventHandler : IConsumer<OrderCreatedEvent>
{
    private readonly CartDbContext _dbContext;

    public OrderCreatedEventHandler(CartDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        try
        {
            var cart = new CartItem
            {
                Id = Guid.NewGuid(),
                ProductName = context.Message.ProductName,
                Price = context.Message.Price
            };
            
            _dbContext.Add(cart);

            await _dbContext.SaveChangesAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}