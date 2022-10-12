using EscortBookCustomerProfile.Handlers;

namespace EscortBookCustomerProfile.Common;

public static class Emitter<T> where T : class
{
    public static void EmitMessage(IOperationHandler<T> operationHandler, T message)
        => operationHandler.Publish(message);
}
