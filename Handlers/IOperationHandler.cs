using System;

namespace EscortBookCustomerProfile.Handlers
{
    public interface IOperationHandler<T> where T : class
    {
        void Publish(T eventType);

        void Subscribe(string subscriberName, Action<T> action);
    }
}
