using System;

namespace EscortBookCustomerProfile.Web.Constants;

public static class Kafka
{
    public static readonly string Servers = Environment.GetEnvironmentVariable("KAFKA_SERVERS");

    public static readonly string ClientId = Environment.GetEnvironmentVariable("KAFKA_CLIENT_ID");
}

public static class KafkaTopic
{
    public const string LockUser = "block-user";

    public const string UserDeleteAccount = "user-delete-account";
}
