using MassTransit;

namespace OrchestratedSagaApp
{
    public class NewsLetterOnBoardSagaData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public Guid SubscriberId { get; set; }
        public string Email { get; set; }
        public bool WelcomeEmailSent { get; set; }
        public bool OnboardingCompletedSent { get; set; }
    }
}