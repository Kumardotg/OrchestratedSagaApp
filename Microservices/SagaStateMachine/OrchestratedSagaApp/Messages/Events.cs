namespace OrchestratedSagaApp
{
    public class SubscriberCreated
    {
        public string Email { get; set; }
        public Guid SubscriberId { get; set; }
    }

    public class WelcomeEmailSent
    {
        public string Email { get; set; }
        public Guid SubscriberId { get; set; }
    }

}