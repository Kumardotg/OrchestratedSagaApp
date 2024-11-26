using MassTransit;

namespace OrchestratedSagaApp
{
    public class WelcomeEmailHandler : IConsumer<SendWelcomeEmail>
    {
        public  async Task Consume(ConsumeContext<SendWelcomeEmail> context)
        {
          await context.Publish(new  WelcomeEmailSent
            {
                Email = context.Message.Email,
                SubscriberId = context.Message.SubscriberId
            });
        }
    }
}