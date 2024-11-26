using MassTransit;

namespace OrchestratedSagaApp
{
    public class NewsLetterOnBoardSaga : MassTransitStateMachine<NewsLetterOnBoardSagaData>
    {
        public State Welcoming {get; set;}
        public State OnBoarding {get; set;}

        public Event<SubscriberCreated> SubscriberCreated {get; set;}
        public Event<WelcomeEmailSent>  WelcomeEmailSent {get; set;}

        public NewsLetterOnBoardSaga(){
            InstanceState(x=> x.CurrentState);

            Event(()=>SubscriberCreated, x=>x.CorrelateById(m=>m.Message.SubscriberId));

            Event(()=>WelcomeEmailSent, x=>x.CorrelateById(m=>m.Message.SubscriberId));

            Initially(
                When(SubscriberCreated)
                .Then(context=> {
                    context.Saga.SubscriberId = context.Message.SubscriberId;
                    context.Saga.Email= context.Message.Email;
                })
                .TransitionTo(Welcoming)
                .Publish(context=> new SendWelcomeEmail(context.Message.SubscriberId, context.Message.Email)));

            During(Welcoming
                ,When(WelcomeEmailSent)
                .Then(context=> {
                    context.Saga.WelcomeEmailSent = true;
                    context.Saga.OnboardingCompletedSent = true;
                })
                .TransitionTo(OnBoarding)
                .Publish(context=> new Onboarding(context.Message.SubscriberId, context.Message.Email))
                .Finalize());
        }
    }
}