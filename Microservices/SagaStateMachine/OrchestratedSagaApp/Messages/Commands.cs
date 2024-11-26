namespace OrchestratedSagaApp
{
    public record SubscribeToNewsLetter(string Email);
    public record SendWelcomeEmail(Guid SubscriberId, string Email);

    public record Onboarding(Guid SubscriberId, string Email);
}