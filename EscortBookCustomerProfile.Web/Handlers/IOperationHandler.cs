using System;

namespace EscortBookCustomerProfile.Web.Handlers;

public interface IOperationHandler<T> where T : class
{
    void Publish(T eventType);

    void Subscribe(string subscriberName, Action<T> action);
}
