using MassTransit;

namespace OrchestratedSagaApp
{
    public class OnboardingCompletedHandler(ILogger<OnboardingCompletedHandler> logger) : IConsumer<Onboarding>
    {
        public Task Consume(ConsumeContext<Onboarding> context)
        {
          logger.LogInformation("On boarding completed");
          return Task.CompletedTask;    
        }
    }
}