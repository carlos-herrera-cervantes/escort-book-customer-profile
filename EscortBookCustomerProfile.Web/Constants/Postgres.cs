using System;

namespace EscortBookCustomerProfile.Web.Constants;

public static class PostgresDatabase
{
	public static readonly string CustomerProfile = Environment.GetEnvironmentVariable("PG_DB_CONNECTION");
}
