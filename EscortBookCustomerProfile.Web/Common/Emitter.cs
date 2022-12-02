using EscortBookCustomerProfile.Web.Handlers;

namespace EscortBookCustomerProfile.Web.Common;

public static class Emitter<T> where T : class
{
    public static void EmitMessage(IOperationHandler<T> operationHandler, T message)
        => operationHandler.Publish(message);
}
