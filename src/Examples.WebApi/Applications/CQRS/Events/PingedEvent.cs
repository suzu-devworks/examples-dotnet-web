using MediatR;

namespace Examples.WebApi.Applications.CQRS.Events
{
    public class PingedEvent : INotification
    {
    }

    public class PingedExtendEvent : PingedEvent
    {
    }

}