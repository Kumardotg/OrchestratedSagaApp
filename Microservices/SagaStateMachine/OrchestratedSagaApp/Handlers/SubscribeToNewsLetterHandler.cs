using MassTransit;

namespace OrchestratedSagaApp
{
    public class SubscribeToNewsLetterHandler : IConsumer<SubscribeToNewsLetter>
    {
        public async Task Consume(ConsumeContext<SubscribeToNewsLetter> context)
        {
            await context.Publish(new SubscriberCreated
            {
                Email = context.Message.Email,
                SubscriberId =  Guid.NewGuid()
            });
        }
    }
}